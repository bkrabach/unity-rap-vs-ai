using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Orchestration")]
    [UnitTitle("Get Context Result")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class ContextGetResult : Unit
    {
        [DoNotSerialize]
        public ValueInput context;

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
            context = ValueInput<SKContext>("context");
        
            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<string>("output", (flow) =>
            {
                var contextValue = flow.GetValue<SKContext>(context);

                return contextValue.Result;
            });

            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
