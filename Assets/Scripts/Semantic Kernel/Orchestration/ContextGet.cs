using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Scripts.SemanticKernel.Orchestration
{
    [UnitCategory("Semantic Kernel/Orchestration")]
    [UnitTitle("Get Context Variables")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class ContextGet : Unit
    {
        [DoNotSerialize]
        public ValueInput context;

        [DoNotSerialize]
        public ValueInput name;

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
            name = ValueInput<string>("name", string.Empty);
            
            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<string>("output", (flow) =>
            {
                var contextValue = flow.GetValue<SKContext>(context);
                var nameValue = flow.GetValue<string>(name);

                return contextValue[nameValue];
            });

            Requirement(name, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
