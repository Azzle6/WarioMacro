using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    [HideInInspector]
    public NodeVisual currentNode;
    public Transform nodesParent;
    public NodeVisual.Path currentPath { get; private set; }

    [SerializeField] public NodeVisual startNode;
    [SerializeField] private NodeVisual endNode;
    
    private static readonly int current = Animator.StringToHash("Current");
    private Player player;

    
    public void Load()
    {
        player = GameController.instance.player;
        gameObject.SetActive(true);
        currentNode = startNode;
        player.TeleportPlayer(currentNode.transform.position);
    }

    public void Unload()
    {
        gameObject.SetActive(false);
    }

    public bool OnLastNode()
    {
        return currentNode == endNode;
    }
    
    public IEnumerator WaitForNodeSelection()
    {
        // init
        var arrowPrefabs = player.arrowPrefabs.ToList();
        var nextNode = default(NodeVisual);
        var nextPath = default(NodeVisual.Path);
        var selectedNode = default(NodeVisual);
        var selectedPath = default(NodeVisual.Path);
        var lastDirectionSelected = MoveDirection.None;
        const ControllerKey validInput = ControllerKey.A;

        arrowPrefabs.ForEach(go => go.SetActive(true));
        
        currentNode.animator.SetBool(current, true);
        
        // input loop
        while (nextNode == null)
        {
            //while (Ticker.lockTimescale) yield return null;

            MoveDirection selectedDirection = InputManager.GetDirection(false, false);
            
            // ReSharper disable once PossibleNullReferenceException
            foreach (NodeVisual.Path path in currentNode.paths.Where(p => p != null))
            {
                if (selectedDirection == MoveDirection.None || path.direction != selectedDirection) continue;
                
                selectedNode = path.destination;
                selectedPath = path;
                if (selectedDirection != lastDirectionSelected) AudioManager.MacroPlaySound("NodeDirection", 0);
                lastDirectionSelected = selectedDirection;
                break;
            }

            selectedDirection = lastDirectionSelected;
            if(selectedPath!=null)
                currentNode.pathRenderer.CreatePathRenderer(selectedPath);
            for (var i = 0; i < arrowPrefabs.Count; i++)
            {
                // is any path setup with arrow direction?
                arrowPrefabs[i].gameObject.SetActive(currentNode.paths.FirstOrDefault(p => p != null && p.direction == (MoveDirection) i) != null);
                // is the selected direction equals to the path direction?
                if (selectedDirection != MoveDirection.None && i == (int) selectedDirection)
                {
                    arrowPrefabs[i].transform.localScale = Vector3.one;
                    arrowPrefabs[i].transform.GetChild(0).gameObject.SetActive(false);
                    arrowPrefabs[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    arrowPrefabs[i].transform.localScale = Vector3.one * .5f;
                    arrowPrefabs[i].transform.GetChild(0).gameObject.SetActive(true);
                    arrowPrefabs[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                
            }
            if (selectedNode != null && InputManager.GetKeyDown(validInput))
            {
                nextNode = selectedNode;
                nextPath = selectedPath;
            }
            var nodeInteract = GameController.instance.map.currentNode.GetComponent<InteractibleNode>();
            if (nodeInteract != null && !GameController.isInActionEvent && InputManager.GetKeyDown(ControllerKey.Y))
            {
                nodeInteract.EventInteractible.Invoke();
                GameController.isInActionEvent = true;
                yield return new WaitWhile(() => GameController.isInActionEvent);
            }
            yield return null;
        }

        currentNode.pathRenderer.ClearPath();
        currentNode.animator.SetBool(current, false);
        currentNode = nextNode;
        currentPath = nextPath;

        // dispose
        arrowPrefabs.ForEach(go => go.SetActive(false));
    }

    private void Awake()
    {
        currentNode = startNode;
        
    }
}
