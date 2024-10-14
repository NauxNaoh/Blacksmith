namespace FangsExtension
{
    public static class TimeConvert
    {
        public static string GetTimeConvertMinues(int time)
        {
            string sTime = "";
            int minus = time / 60;
            int second = time % 60;

            string sMinus = $"{minus}";
            if (minus < 10)
                sMinus = "0" + minus;

            string sSecond = $"{second}";
            if (second < 10)
                sSecond = "0" + second;

            sTime = sMinus + ":" + sSecond;


            return sTime;
        }

        public static string GetTimeConvertHour(int time)
        {
            if (time < 0)
                return "00:00:00";
            string sTime = "";
            int hour = time / 3600;
            int minus = (time - hour * 3600) / 60;
            int second = (time - hour * 3600) % 60;

            string sHour = $"{hour}";
            if (hour < 10)
                sHour = "0" + hour;

            string sMinus = $"{minus}";
            if (minus < 10)
                sMinus = "0" + minus;

            string sSecond = $"{second}";
            if (second < 10)
                sSecond = "0" + second;

            sTime = sHour + ":" + sMinus + ":" + sSecond;


            return sTime;
        }
    }
}