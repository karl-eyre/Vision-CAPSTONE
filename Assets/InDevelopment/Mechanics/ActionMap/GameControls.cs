// GENERATED AUTOMATICALLY FROM 'Assets/InDevelopment/Mechanics/ActionMap/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""In-Game"",
            ""id"": ""456d167e-2da9-4a51-8e16-ddb89cf048ec"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""3ba75988-4205-40a8-b11b-44e884f970dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseLook"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4406ad26-d4ed-43d7-b4d0-c892dc0a9359"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""16eb1813-27e1-4852-b0e7-a4b055704191"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VisionAbilityActivation"",
                    ""type"": ""Button"",
                    ""id"": ""816a8bbb-2fe5-4926-908d-7c061f2fb31a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""83334f22-f94e-4223-84e0-9f6a9c0fedd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""69ee7ac9-4131-488c-911a-d12799a5618a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PickUpObject"",
                    ""type"": ""Button"",
                    ""id"": ""d2be3771-6db4-4450-90e8-77b180ab5404"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThrowObject"",
                    ""type"": ""Button"",
                    ""id"": ""c6a7dded-7f7c-4805-8234-58cbe746ae83"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""4aefdafe-a308-497d-bb03-3f7d66212e65"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""58b68938-92ae-4d50-9be9-ee8393c3e521"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""58cdd27d-e6b2-4993-83ba-b883a14c7fc9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4957299f-0c52-4d0d-bfd3-28a67ab2dd17"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""baee2571-5d37-4439-b215-367c39ef543a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""24493054-1ef8-4169-9730-6d34a1672389"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba37a40b-bad6-4050-86fa-d8a89702a0d0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VisionAbilityActivation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63f1c855-7b13-4a27-8ba8-da5f25909580"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e589788-01be-41f2-ba05-e2e76c028084"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4e6335b-0b12-426f-8015-352f8b105991"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8abcf6c-cfe9-4e9b-9eb4-a5b769ac89de"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickUpObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b26235a-829f-4a72-956e-a31bbc04f45a"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""229d70e7-f3d7-4407-ad89-42d46cb69ed0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // In-Game
        m_InGame = asset.FindActionMap("In-Game", throwIfNotFound: true);
        m_InGame_Movement = m_InGame.FindAction("Movement", throwIfNotFound: true);
        m_InGame_MouseLook = m_InGame.FindAction("MouseLook", throwIfNotFound: true);
        m_InGame_MousePosition = m_InGame.FindAction("MousePosition", throwIfNotFound: true);
        m_InGame_VisionAbilityActivation = m_InGame.FindAction("VisionAbilityActivation", throwIfNotFound: true);
        m_InGame_Jump = m_InGame.FindAction("Jump", throwIfNotFound: true);
        m_InGame_Crouch = m_InGame.FindAction("Crouch", throwIfNotFound: true);
        m_InGame_PickUpObject = m_InGame.FindAction("PickUpObject", throwIfNotFound: true);
        m_InGame_ThrowObject = m_InGame.FindAction("ThrowObject", throwIfNotFound: true);
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

    // In-Game
    private readonly InputActionMap m_InGame;
    private IInGameActions m_InGameActionsCallbackInterface;
    private readonly InputAction m_InGame_Movement;
    private readonly InputAction m_InGame_MouseLook;
    private readonly InputAction m_InGame_MousePosition;
    private readonly InputAction m_InGame_VisionAbilityActivation;
    private readonly InputAction m_InGame_Jump;
    private readonly InputAction m_InGame_Crouch;
    private readonly InputAction m_InGame_PickUpObject;
    private readonly InputAction m_InGame_ThrowObject;
    public struct InGameActions
    {
        private @GameControls m_Wrapper;
        public InGameActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_InGame_Movement;
        public InputAction @MouseLook => m_Wrapper.m_InGame_MouseLook;
        public InputAction @MousePosition => m_Wrapper.m_InGame_MousePosition;
        public InputAction @VisionAbilityActivation => m_Wrapper.m_InGame_VisionAbilityActivation;
        public InputAction @Jump => m_Wrapper.m_InGame_Jump;
        public InputAction @Crouch => m_Wrapper.m_InGame_Crouch;
        public InputAction @PickUpObject => m_Wrapper.m_InGame_PickUpObject;
        public InputAction @ThrowObject => m_Wrapper.m_InGame_ThrowObject;
        public InputActionMap Get() { return m_Wrapper.m_InGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
        public void SetCallbacks(IInGameActions instance)
        {
            if (m_Wrapper.m_InGameActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                @MouseLook.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMouseLook;
                @MouseLook.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMouseLook;
                @MouseLook.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMouseLook;
                @MousePosition.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMousePosition;
                @VisionAbilityActivation.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnVisionAbilityActivation;
                @VisionAbilityActivation.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnVisionAbilityActivation;
                @VisionAbilityActivation.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnVisionAbilityActivation;
                @Jump.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnJump;
                @Crouch.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnCrouch;
                @PickUpObject.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPickUpObject;
                @PickUpObject.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPickUpObject;
                @PickUpObject.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPickUpObject;
                @ThrowObject.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnThrowObject;
                @ThrowObject.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnThrowObject;
                @ThrowObject.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnThrowObject;
            }
            m_Wrapper.m_InGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @MouseLook.started += instance.OnMouseLook;
                @MouseLook.performed += instance.OnMouseLook;
                @MouseLook.canceled += instance.OnMouseLook;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @VisionAbilityActivation.started += instance.OnVisionAbilityActivation;
                @VisionAbilityActivation.performed += instance.OnVisionAbilityActivation;
                @VisionAbilityActivation.canceled += instance.OnVisionAbilityActivation;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @PickUpObject.started += instance.OnPickUpObject;
                @PickUpObject.performed += instance.OnPickUpObject;
                @PickUpObject.canceled += instance.OnPickUpObject;
                @ThrowObject.started += instance.OnThrowObject;
                @ThrowObject.performed += instance.OnThrowObject;
                @ThrowObject.canceled += instance.OnThrowObject;
            }
        }
    }
    public InGameActions @InGame => new InGameActions(this);
    public interface IInGameActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnMouseLook(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnVisionAbilityActivation(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnPickUpObject(InputAction.CallbackContext context);
        void OnThrowObject(InputAction.CallbackContext context);
    }
}
