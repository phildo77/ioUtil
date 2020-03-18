using System.Diagnostics;

namespace ioSS.Util
{
    public class Progress
    {
        public delegate void OnUpdate(float _progPct, string _progStr);

        private OnUpdate m_OnProgUpdate;
        private readonly string m_ProgTag;
        private readonly Stopwatch m_StopWatch;
        public int UpdateRateMilli = 250;

        public Progress(string _tag = "Progress")
        {
            m_ProgTag = _tag;
            m_StopWatch = new Stopwatch();
            m_StopWatch.Start();
        }

        public float GetProgPct { get; private set; }

        public string GetProgStr { get; private set; } = "";

        public void SetOnUpdate(OnUpdate _onUpdate)
        {
            m_OnProgUpdate = _onUpdate;
        }

        public void Update(float _progPct, string _progStr, bool _force = false)
        {
            GetProgPct = _progPct; //TODO inefficient? put after if statement?
            GetProgStr = _progStr;
            if (m_StopWatch.ElapsedMilliseconds < UpdateRateMilli && _force == false) return;
            m_OnProgUpdate(GetProgPct, m_ProgTag + ": " + GetProgStr);

            m_StopWatch.Restart();
        }

        public void Update(float _progPct)
        {
            Update(_progPct, GetProgStr);
        }
    }
}