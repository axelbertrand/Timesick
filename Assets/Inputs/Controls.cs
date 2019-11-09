// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""QTE"",
            ""id"": ""843c19d6-6d92-499d-9730-89c07458dff3"",
            ""actions"": [
                {
                    ""name"": ""ButtonSouth"",
                    ""type"": ""Button"",
                    ""id"": ""aaba128f-5f35-48cc-8783-cb6f1867cd79"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonNorth"",
                    ""type"": ""Button"",
                    ""id"": ""fe61af5a-8bc3-4c70-b5a9-6afccc42005c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonWest"",
                    ""type"": ""Button"",
                    ""id"": ""48b65a87-4e59-4b42-9d90-e4a6a9347118"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonEast"",
                    ""type"": ""Button"",
                    ""id"": ""fe0466cf-238c-4bce-b0fc-7a9d24aa0408"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8f559ec0-5d37-4ac7-ac63-cfa6a0fd466f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23885bc2-c9a6-4549-88fa-00f0992f8b1f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""856cc6a8-3e06-4b50-a3eb-79ebdb9d9d18"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58b2a2cc-5c68-4d25-961a-1f5de6888178"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae7877da-d87d-4f72-9745-5643f25dc992"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e685b867-5dce-400d-89f6-1d914e6b07de"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2863ed68-ddd6-462f-a65f-8356a19b5c2c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e94544d0-d2a7-410b-92e1-ab6284c59ecd"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // QTE
        m_QTE = asset.FindActionMap("QTE", throwIfNotFound: true);
        m_QTE_ButtonSouth = m_QTE.FindAction("ButtonSouth", throwIfNotFound: true);
        m_QTE_ButtonNorth = m_QTE.FindAction("ButtonNorth", throwIfNotFound: true);
        m_QTE_ButtonWest = m_QTE.FindAction("ButtonWest", throwIfNotFound: true);
        m_QTE_ButtonEast = m_QTE.FindAction("ButtonEast", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // QTE
    private readonly InputActionMap m_QTE;
    private IQTEActions m_QTEActionsCallbackInterface;
    private readonly InputAction m_QTE_ButtonSouth;
    private readonly InputAction m_QTE_ButtonNorth;
    private readonly InputAction m_QTE_ButtonWest;
    private readonly InputAction m_QTE_ButtonEast;
    public struct QTEActions
    {
        private @Controls m_Wrapper;
        public QTEActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ButtonSouth => m_Wrapper.m_QTE_ButtonSouth;
        public InputAction @ButtonNorth => m_Wrapper.m_QTE_ButtonNorth;
        public InputAction @ButtonWest => m_Wrapper.m_QTE_ButtonWest;
        public InputAction @ButtonEast => m_Wrapper.m_QTE_ButtonEast;
        public InputActionMap Get() { return m_Wrapper.m_QTE; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(QTEActions set) { return set.Get(); }
        public void SetCallbacks(IQTEActions instance)
        {
            if (m_Wrapper.m_QTEActionsCallbackInterface != null)
            {
                @ButtonSouth.started -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.performed -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.canceled -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonSouth;
                @ButtonNorth.started -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.performed -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.canceled -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonNorth;
                @ButtonWest.started -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonWest;
                @ButtonWest.performed -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonWest;
                @ButtonWest.canceled -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonWest;
                @ButtonEast.started -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonEast;
                @ButtonEast.performed -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonEast;
                @ButtonEast.canceled -= m_Wrapper.m_QTEActionsCallbackInterface.OnButtonEast;
            }
            m_Wrapper.m_QTEActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ButtonSouth.started += instance.OnButtonSouth;
                @ButtonSouth.performed += instance.OnButtonSouth;
                @ButtonSouth.canceled += instance.OnButtonSouth;
                @ButtonNorth.started += instance.OnButtonNorth;
                @ButtonNorth.performed += instance.OnButtonNorth;
                @ButtonNorth.canceled += instance.OnButtonNorth;
                @ButtonWest.started += instance.OnButtonWest;
                @ButtonWest.performed += instance.OnButtonWest;
                @ButtonWest.canceled += instance.OnButtonWest;
                @ButtonEast.started += instance.OnButtonEast;
                @ButtonEast.performed += instance.OnButtonEast;
                @ButtonEast.canceled += instance.OnButtonEast;
            }
        }
    }
    public QTEActions @QTE => new QTEActions(this);
    public interface IQTEActions
    {
        void OnButtonSouth(InputAction.CallbackContext context);
        void OnButtonNorth(InputAction.CallbackContext context);
        void OnButtonWest(InputAction.CallbackContext context);
        void OnButtonEast(InputAction.CallbackContext context);
    }
}