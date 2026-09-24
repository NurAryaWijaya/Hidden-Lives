using System.Collections.Generic;

namespace Game.Flow
{
    // Interface untuk node yang memiliki output.
    // Digunakan oleh Flow Editor untuk menggambar connection.
    public interface IFlowOutput
    {
        IEnumerable<FlowNode> GetOutputs();
        void SetOutput(int index, FlowNode node);
        void RemoveOutput(FlowNode node);
    }
}