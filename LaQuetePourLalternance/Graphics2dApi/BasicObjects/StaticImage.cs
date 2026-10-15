namespace LaQuetePourLalternance.Graphics2dApi.BasicObjects;

public class StaticImage : Square
{
    public StaticImage(SquareData data) : base(data)
    {
        Data.ScaleAccordingToTex(0.3f);
    }
}