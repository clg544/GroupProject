using UnityEngine;
using System.Collections.Generic;

namespace NG
{
	public class SceneGenerator : MonoBehaviour
	{
		/// <summary>
		/// The number of iterations the generator should go through.
		/// </summary>
		public int iterations = 5;

		/// <summary>
		/// the AreaData templates that will be used in generating the scene.
		/// </summary>
		public AreaData[] areaDatas;

		/// <summary>
		/// The areas generated by Generate(). 
		/// </summary>
		public List<AreaData> generatedAreas { get; set; }

		public Material material;

		public int dungeonHeight;
		public int dungeonWidth;

		public MeshFilter walls;
		public MeshFilter dungeon;

		public GameObject startPoint;

		void Generate()
		{
			generatedAreas = new List<AreaData>();

			// Select a random area to start with.
			int randomAreaIndex = Random.Range(0, areaDatas.Length);
			AreaData randomArea = areaDatas[randomAreaIndex];

			// Create a copy of the AreaData.
			AreaData newArea = new AreaData(randomArea.name, randomArea.availableTransitions);

			for (int i = 0; i < iterations; i++)
			{
				// Iterate through this area's transitions and add connections
				// however, we'll also need to check for existing areas blocking the way or that should be connected
				for (int j = 0; j < newArea.availableTransitions.Length; j++)
				{
					Direction transition = newArea.availableTransitions[j];

					if (transition == Direction.None)
						continue;

					Direction opposite = transition.GetOpposite();

					Coordinates adjacentAreaCoordinate = newArea.coordinates.GetAdjacentCoordinate(transition);
					AreaData adjacentArea = GetGeneratedAreaByCoordinate(adjacentAreaCoordinate);

					// if there's an area in the way check if it has an available transition opposite of this transition.
					if (adjacentArea != null)
					{
						if (!adjacentArea.GetIsTransitionAvailable(opposite))
						{
							// The adjecent area cannot be transitioned to from this area.
							adjacentArea = null;

							// We should actually now flag this direction as no longer viable.
							newArea.availableTransitions[j] = Direction.None;
						}
					}
					// otherwise create a new area
					else
					{
						adjacentArea = CreateRandomAreaWithTransition(opposite);

						if (adjacentArea == null)
						{
							Debug.LogErrorFormat(
								"Could not GetRandomAreaWithTransition({0}). " +
								"Please ensure areaDatas has available transitions on all sides",
								opposite);
						}
						else
						{
							adjacentArea.coordinates = adjacentAreaCoordinate;
							generatedAreas.Add(adjacentArea);
						}
					}

					if (adjacentArea != null)
					{
						// assign the connection between the two areas.
						newArea.SetTransitionUsed(transition, adjacentArea, opposite);
						adjacentArea.SetTransitionUsed(opposite, newArea, transition);
					}
				}

				// check to see if we assigned any transitions to this new area, if so add it to the generatedAreas list.
				if (newArea.GetTransitionCount() > 0)
				{
					if (!generatedAreas.Contains(newArea))
						generatedAreas.Add(newArea);
				}
				// otherwise did something go wrong?
				else
				{
					Debug.LogWarning("No transitions assigned to area: " + newArea.ToString());
				}

				// Now we need to get the next area to work on.
				newArea = null;
				foreach (var item in generatedAreas)
				{
					if (item.HasAnyAvailableTransition())
					{
						newArea = item;
						break;
					}
				}

				if (newArea == null)
				{
					Debug.Log("Can't find any generated areas with avilable transitions. Quitting.");
					break;
				}
			}
		}


		/// <summary>
		/// Checks the list of generated areas to see if one exists at the supplied coordinates.
		/// </summary>
		/// <param name="coordinates">Coordinates to check if an area exists at.</param>
		/// <returns>An area from the generated areas list matching the supplied coordinates. 
		/// If none is found, then null is returned.</returns>
		private AreaData GetGeneratedAreaByCoordinate(Coordinates coordinates)
		{
			foreach (var item in generatedAreas)
			{
				if (item.coordinates.x == coordinates.x && item.coordinates.y == coordinates.y)
					return item;
			}

			return null;
		}


		/// <summary>
		/// Creates a new random area with the indicated position available.
		/// </summary>
		/// <param name="transition">The transition that needs to be available on the area.</param>
		/// <returns>A new AreaData with matching transition. If none can be found then null is returned.</returns>
		private AreaData CreateRandomAreaWithTransition(Direction transition)
		{
			int areaDatasIndex = Random.Range(0, areaDatas.Length);

			//Debug.Log("transition to look for: " + transition);

			for (int i = 0; i < areaDatas.Length; i++)
			{
				bool isTransitionAvailable = areaDatas[areaDatasIndex].GetIsTransitionAvailable(transition);
				//Debug.LogFormat("areaDatasIndex: {0}  areaData {1}  available: {2}", areaDatasIndex, areaDatas[areaDatasIndex], isTransitionAvailable);

				if (isTransitionAvailable)
					return new AreaData(
						areaDatas[areaDatasIndex].name,
						areaDatas[areaDatasIndex].availableTransitions);

				areaDatasIndex++;
				if (areaDatasIndex == areaDatas.Length)
					areaDatasIndex = 0;
			}

			return null;
		}


		void Start()
		{
			Generate();

			CreateRepresentation();
		}


		/// <summary>
		/// Creates cubes for each generated area and cubes to show the transitions between each.
		/// Transitions are offset so that we can see 1 exists in each direction (to/from).
		/// </summary>
		void CreateRepresentation()
		{
			for (int i = 0; i < generatedAreas.Count; i++)
			{
				GameObject section = new GameObject();
				// Attach an Area component so we can easily inspect the AreaData in the editor.
				section.AddComponent<LevelGenerator>();
				section.AddComponent <MeshCreator>();
				section.AddComponent <MeshFilter>();
				section.AddComponent<MeshRenderer>();

				section.GetComponent<MeshRenderer> ().material = material;
				section.GetComponent<LevelGenerator> ().randomFillPercent = Random.Range (40, 45);
				section.GetComponent<LevelGenerator> ().width = dungeonWidth;
				section.GetComponent<LevelGenerator> ().height = dungeonHeight;
				section.GetComponent<LevelGenerator> ().useRandomSeed = true;

				//section.GetComponent<LevelGenerator> ().startPoint = startPoint;

				Area area = section.AddComponent<Area>();

				generatedAreas[i].name = i + " - " + generatedAreas[i].name;
				area.areaData = generatedAreas[i];
				section.transform.position = generatedAreas[i].coordinates.ToVector2();
				section.transform.localScale = 0.75f * Vector3.one;
				section.name = generatedAreas[i].name;
				area.transform.Rotate (-90, 0, 0);


				foreach (var item in generatedAreas[i].transitions)
				{
					Vector2 transitionPostion =
						0.5f * (generatedAreas[i].coordinates.ToVector2() + item.Value.Key.coordinates.ToVector2());

					GameObject transition = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Vector3 scale = 2f * Vector3.one;
					transition.name = item.Key.ToString() + " to " + item.Value.Value.ToString();

					switch (item.Key)
					{
						case Direction.N:
							transitionPostion.x += 10f;
							scale.y = 10f;
							break;
						case Direction.E:
							transitionPostion.y += 10f;
							scale.x = 10f;
							break;
						case Direction.S:
							scale.y = 10f;
							transitionPostion.x -= 10f;
							break;
						case Direction.W:
							transitionPostion.y -= 10f;
							scale.x = 10f;
							break;
						default:
							break;
					}

					transition.transform.position = transitionPostion;
					transition.transform.localScale = scale;
					transition.transform.SetParent(section.transform, true);
				}
			}
		}
	}
}
