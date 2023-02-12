namespace websocket_server
{
    public class EngineInfo
    {
        public Guid MachineId { get; set; }
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public EngineStatus Status { get; set; }
    }

    public enum EngineStatus : byte
    {
        Idle = 0,
        Running = 1,
        Finished = 2,
        Errored = 3
    }
}
