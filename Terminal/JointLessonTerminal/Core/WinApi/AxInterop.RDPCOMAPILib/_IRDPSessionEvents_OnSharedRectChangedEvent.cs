namespace AxRDPCOMAPILib
{
    internal class _IRDPSessionEvents_OnSharedRectChangedEvent
    {
        public int left;

        public int top;

        public int right;

        public int bottom;

        public _IRDPSessionEvents_OnSharedRectChangedEvent(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }
}