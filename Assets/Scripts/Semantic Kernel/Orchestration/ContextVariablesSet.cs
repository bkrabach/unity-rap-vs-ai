using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Orchestration")]
    [UnitTitle("Set Context Variables")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class ContextVariablesSet : Unit
    {
        [DoNotSerialize]
        public ValueInput context;

        [DoNotSerialize]
        public ValueInput name;

        [DoNotSerialize]
        public ValueInput value;

        [DoNotSerialize]
        public ValueOutput output;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput enter;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput exit;

        protected override void Definition()
        {
            context = ValueInput<ContextVariables>("context");
            name = ValueInput<string> ("name", string.Empty);
            value = ValueInput<string>("value", null);

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<ContextVariables>("output", (flow) =>
            {
                var contextValue = flow.GetValue<ContextVariables>(context);
                var nameValue = flow.GetValue<string>(name);
                var valueValue = flow.GetValue<string>(value);
                
                contextValue.Set(nameValue, valueValue);
                return contextValue;
            });

            Requirement(name, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
