using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueGraphView : GraphView
{
    public readonly Vector2 defultNodeSize = new Vector2(150, 200);

    public DialogueGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);

        grid.StretchToParentSize();

    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    
   
    public void CreateNode(string nodeName, Color colorInput, Color colorOutput )
    {
        AddElement(CreateDialogueNode(nodeName, colorInput, colorOutput));
    }

    public DialogueNode CreateDialogueNode(string nodeName, Color colorInput, Color colorOutput)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogText = nodeName,
            GUID = Guid.NewGuid().ToString(),
            inputColor = colorInput,
            outputColor = colorOutput
        };
        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = Constants.INPUT;
        inputPort.portColor = dialogueNode.inputColor;
        dialogueNode.inputContainer.Add(inputPort);

        var button = new Button(()=> { AddChoicePort(dialogueNode); });
        button.text = "New Output";
        dialogueNode.titleContainer.Add(button);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defultNodeSize));

        return dialogueNode;
    }


    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "" )
    {
        var generatePort = GeneratePort(dialogueNode, Direction.Output);
        var oldLable = generatePort.contentContainer.Q<Label> ("type");
        generatePort.contentContainer.Remove(oldLable);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        generatePort.portName = Constants.OUTPUT;
        generatePort.portColor = dialogueNode.outputColor;
        var choicePortName = string.IsNullOrEmpty(overriddenPortName) ? Constants.OUTPUT : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        generatePort.contentContainer.Add(new Label(" "));
        generatePort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(dialogueNode, generatePort))
        {
            text = "X"
        };
        generatePort.contentContainer.Add(deleteButton);

        generatePort.portName = choicePortName;
        dialogueNode.outputContainer.Add(generatePort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatePort)
    {
        var targetEdge = edges.ToList().
            Where(x => x.output.portName == generatePort.portName && x.output.node == generatePort.node);

        if (!targetEdge.Any()) return;
        var edge = targetEdge.First();
        edge.input.Disconnect(edge);
        RemoveElement(targetEdge.First());

        dialogueNode.outputContainer.Remove(generatePort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    public override List<Port> GetCompatiblePorts(Port startPoint, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPoint != port && startPoint.node != port.node && startPoint.portColor == port.portColor )
            if(!startPoint.portName.Equals(port.portName))
                compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }
}
