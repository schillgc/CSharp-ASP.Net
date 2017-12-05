namespace TreehouseDefense
{
    class LongRangeTower : Tower
    {
        protected override int Range { get; } = 2;
        
        protected override double Accuracy { get; } = 0.5;
        
        public LongRangeTower(MapLocation location) : base(location)
        {}
    }
}