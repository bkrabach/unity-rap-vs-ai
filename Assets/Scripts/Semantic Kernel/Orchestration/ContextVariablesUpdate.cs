using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Orchestration")]
    [UnitTitle("Update Context Variables")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class ContextVariablesUpdate : Unit
    {
        [DoNotSerialize]
        public ValueInput context;

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
            value = ValueInput<string>("value");

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<ContextVariables>("output", (flow) =>
            {
                var contextValue = flow.GetValue<ContextVariables>(context);
                var valueValue = flow.GetValue<string>(value);

                contextValue.Update(valueValue);
                return contextValue;
            });

            Requirement(context, enter);
            Requirement(value, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
