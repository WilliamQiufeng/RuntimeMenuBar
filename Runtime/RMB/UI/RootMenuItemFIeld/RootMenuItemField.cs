using RMB.SO;
using UnityEngine.UIElements;

namespace RMB.UI.RootMenuItemFIeld
{
    public class RootMenuItemField : BaseField<RootMenuItemSO>
    {
        // We can provide the existing BaseFieldTraits class as a type parameter for UxmlFactory, and this means we
        // don't need to define our own traits class or override its Init() method. We do, however, need to provide it
        // However, you must provide the value type (double) and its attribute description type:
        // (UxmlDoubleAttributeDescription).
        public new class UxmlFactory :
            UxmlFactory<RootMenuItemField, BaseFieldTraits<RootMenuItemSO, UxmlObjAttributeDescription<RootMenuItemSO>>> { }

        private Button m_Button;

        // Default constructor is required for compatibility with UXML factory
        public RootMenuItemField() : this(null)
        {

        }

        // Main constructor accepts label parameter to mimic BaseField constructor.
        // Second argument to base constructor is the input element, the one that displays the value this field is
        // bound to.
        public RootMenuItemField(string label) : base(label, new Button())
        {
            // This is the input element instantiated for the base constructor.
            m_Button = this.Q<Button>(className: inputUssClassName);
        }

        // SetValueWithoutNotify needs to be overridden by calling the base version and then making a change to the
        // underlying value be reflected in the input element.
        public override void SetValueWithoutNotify(RootMenuItemSO newValue)
        {
            base.SetValueWithoutNotify(newValue);

            m_Button.text = newValue.name;
        }
    }
}