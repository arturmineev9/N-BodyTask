namespace NBody;

public static class Helpers
{
    public static int[][] Ranges(int startNum, int endNum, int rangesNum)
    {
        float lowLimit = startNum;
        float partition = (endNum - lowLimit) / rangesNum;
        
        int[][] ranges = new int[rangesNum][];
        ranges[0] = new int[2];
        ranges[0][0] = (int)Math.Round(lowLimit);

        for (int rowIndex = 0; rowIndex < rangesNum - 1; rowIndex++)
        {
            lowLimit += partition;
            ranges[rowIndex][1] = (int)Math.Round(lowLimit);
            ranges[rowIndex + 1] = new int[2];
            ranges[rowIndex + 1][0] = ranges[rowIndex][1] + 1;
        }

        ranges[rangesNum - 1][1] = endNum;

        return ranges;
    }
}