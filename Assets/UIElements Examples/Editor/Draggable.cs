using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Draggable : MouseManipulator
{
    Action<Vector2> m_Handler;

    bool m_Active;

    bool m_OutputDeltaMovement;

    public Draggable(Action<Vector2> handler, bool outputDeltaMovement = false)
    {
        m_Handler = handler;
        m_Active = false;
        m_OutputDeltaMovement = outputDeltaMovement;
        activators.Add(new ManipulatorActivationFilter()
        {
            button = MouseButton.LeftMouse
        });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback(new EventCallback<MouseDownEvent>(OnMouseDown));
        target.RegisterCallback(new EventCallback<MouseMoveEvent>(OnMouseMove));
        target.RegisterCallback(new EventCallback<MouseUpEvent>(OnMouseUp));
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback(new EventCallback<MouseDownEvent>(OnMouseDown));
        target.UnregisterCallback(new EventCallback<MouseMoveEvent>(OnMouseMove));
        target.UnregisterCallback(new EventCallback<MouseUpEvent>(OnMouseUp));
    }

    void OnMouseDown(MouseDownEvent evt)
    {
        target.TakeMouseCapture();
        m_Active = true;
        evt.StopPropagation();
    }

    void OnMouseMove(MouseMoveEvent evt)
    {
        if (m_Active)
        {
            if (m_OutputDeltaMovement)
            {
                m_Handler(evt.mouseDelta);
            }
            else
            {
                m_Handler(evt.localMousePosition);
            }
        }
    }

    void OnMouseUp(MouseUpEvent evt)
    {
        m_Active = false;

        if (target.HasMouseCapture())
        {
            target.ReleaseMouseCapture();
        }

        evt.StopPropagation();
    }
}