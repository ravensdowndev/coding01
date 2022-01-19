namespace SpreadyMcSpreader.Models
{
    public class FertilizerSpreaderModel
    {
        public FertilizerSpreaderModel(int[,] layout, int initialFertilzer, int[,] remainingFertilzer)
        {
            Layout = layout;
            InitialFertilizer = initialFertilzer;
            RemainingFertilizer = remainingFertilzer;
        }

        public int[,] Layout { get; }
        public int InitialFertilizer { get; }
        public int[,] RemainingFertilizer { get; }
    }
}
