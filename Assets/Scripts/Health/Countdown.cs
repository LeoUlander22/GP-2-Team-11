namespace Team11.Health
{
    public class Countdown
    {
        private readonly float _time;
        private float _currentTime;
        private bool _started;

        public bool IsTicking => _started;
        public float CurrentTime => _currentTime;
        
        public Countdown(float time)
        {
            _time = time;
        }

        public void Tick(float interval)
        {
            if (!_started) return;
            
            _currentTime -= interval;
            if(_currentTime <= 0)
                Stop();
        }

        public void Start()
        {
            _currentTime = _time;
            _started = true;
        }
        
        public void Resume()
        {
            _started = true;
        }
        
        public void Stop()
        {
            _started = false;
        }
    }
}