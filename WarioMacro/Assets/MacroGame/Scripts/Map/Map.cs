using System.Collections;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{ 
    public Node currentNode { get; private set; }
    public Node.Path currentPath { get; private set; }
    
    [SerializeField] private Player player;
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;
    
    private static readonly int current = Animator.StringToHash("Current");


    public void Load()
    {
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
        var nextNode = default(Node);
        var nextPath = default(Node.Path);
        var selectedNode = default(Node);
        var selectedPath = default(Node.Path);
        Node.Direction? lastDirectionSelected = null;
        const ControllerKey validInput = ControllerKey.A;

        arrowPrefabs.ForEach(go => go.SetActive(true));
        
        currentNode.animator.SetBool(current, true);
        
        // input loop
        while (nextNode == null)
        {
            var selectedDirection = Node.GetPlayerDirection();
            
            // ReSharper disable once PossibleNullReferenceException
            foreach (Node.Path path in currentNode.paths)
            {
                if (selectedDirection == null || path.direction != selectedDirection) continue;
                
                selectedNode = path.destination;
                selectedPath = path;
                if (selectedDirection != lastDirectionSelected) AudioManager.MacroPlaySound("MOU_NodeDirection", 0);
                lastDirectionSelected = selectedDirection;
                break;
            }

            selectedDirection = lastDirectionSelected;

            for (var i = 0; i < arrowPrefabs.Count; i++)
            {
                // is any path setup with arrow direction?
                arrowPrefabs[i].gameObject.SetActive(currentNode.paths.FirstOrDefault(p => p.direction == (Node.Direction)i) != null);
                // is the selected direction equals to the path direction?
                arrowPrefabs[i].transform.localScale = (selectedDirection != null && i == (int) selectedDirection) ? Vector3.one : Vector3.one * .5f;
            }
            
            if (selectedNode != null && InputManager.GetKeyDown(validInput))
            {
                nextNode = selectedNode;
                nextPath = selectedPath;
            }
            yield return null;
        }
        
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
