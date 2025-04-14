namespace gLibrary.Models
{
   public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string? Fill { get; set; }
        public string? Text { get; set; }
        public string? Raster { get; set; }

        public Cell(int row, int col, string fill = "#FFFFFF", string text = "", string raster = "")
        {
            this.Row = row;
            this.Column = col;
            this.Fill = fill;
            this.Text = text;
            this.Raster = raster;
        }
    }
}
