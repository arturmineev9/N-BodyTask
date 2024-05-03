namespace NBody;

public static class Helpers
{
    public static int[][] GetRanges(int startNum, int endNum, int rangesNum)
    {
        /*double lowLimit = startNum;
        double partition = (endNum - lowLimit) / rangesNum;
        
        int[][] ranges = new int[rangesNum][];
        ranges[0] = new int[2];
        ranges[0][0] = startNum;

        for (int rowIndex = 0; rowIndex < rangesNum - 1; rowIndex++)
        {
            lowLimit += partition;
            ranges[rowIndex][1] = (int)Math.Round(lowLimit); //((endNum - startNum) % 2 == 0) ? (int)Math.Round(lowLimit) : (int)Math.Floor(lowLimit);
            ranges[rowIndex + 1] = new int[2];
            ranges[rowIndex + 1][0] = ranges[rowIndex][1] + 1;
        }

        ranges[rangesNum - 1][1] = endNum;
*/
        
        
        // Вычисление размера каждого диапазона
        int rangeSize = (endNum - startNum + 1) / rangesNum;
        
        // Массив для хранения диапазонов
        int[][] ranges = new int[rangesNum][];
        
        // Заполнение массива диапазонов
        int currentStart = startNum;
        for (int i = 0; i < rangesNum; i++)
        {
            int currentEnd = currentStart + rangeSize - 1;
            // Учтем остаток при делении
            if (i == rangesNum - 1)
            {
                currentEnd += (endNum - startNum + 1) % rangesNum;
            }
            ranges[i] = new int[] { currentStart, currentEnd };
            currentStart = currentEnd + 1;
        }
        return ranges;
    }
}