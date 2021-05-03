using System;
using System.Collections.Generic;

namespace ZadanieRekrutacyjne
{
    class Program
    {
        static void Main(string[] args)
        {
            //Wejście
            int meeting_duration = 30;

            Calendar.working_hours workig_hours1 = new Calendar.working_hours();
            workig_hours1.start = workig_hours1.start.AddHours(7).AddMinutes(0);
            workig_hours1.end = workig_hours1.end.AddHours(19).AddMinutes(55);

            Calendar.working_hours workig_hours2 = new Calendar.working_hours();
            workig_hours2.start = workig_hours2.start.AddHours(9).AddMinutes(0);
            workig_hours2.end = workig_hours2.end.AddHours(18).AddMinutes(30);

            //Planowane Spotkania - Kalendarz1
            Calendar.planned_meeting planned_meeting1 = new Calendar.planned_meeting();
            planned_meeting1.start.Add(new DateTime().AddHours(9).AddMinutes(0));
            planned_meeting1.end.Add(new DateTime().AddHours(10).AddMinutes(30));

            planned_meeting1.start.Add(new DateTime().AddHours(12).AddMinutes(0));
            planned_meeting1.end.Add(new DateTime().AddHours(13).AddMinutes(0));

            planned_meeting1.start.Add(new DateTime().AddHours(16).AddMinutes(0));
            planned_meeting1.end.Add(new DateTime().AddHours(18).AddMinutes(0));

            //Planowane Spotkania - Kalendarz2
            Calendar.planned_meeting planned_meeting2 = new Calendar.planned_meeting();
            planned_meeting2.start.Add(new DateTime().AddHours(10).AddMinutes(0));
            planned_meeting2.end.Add(new DateTime().AddHours(11).AddMinutes(30));


            planned_meeting2.start.Add(new DateTime().AddHours(12).AddMinutes(30));
            planned_meeting2.end.Add(new DateTime().AddHours(14).AddMinutes(30));

            planned_meeting2.start.Add(new DateTime().AddHours(14).AddMinutes(30));
            planned_meeting2.end.Add(new DateTime().AddHours(15).AddMinutes(0));

            planned_meeting2.start.Add(new DateTime().AddHours(16).AddMinutes(0));
            planned_meeting2.end.Add(new DateTime().AddHours(17).AddMinutes(0));
            //Algorytm
            Calendar.working_hours working_hours_merge = new Calendar.working_hours();
            if (workig_hours1.start.CompareTo(workig_hours2.start) > 0)
            {
                working_hours_merge.start = working_hours_merge.start.AddHours(get_hours(workig_hours1.start.ToString("HH:mm"))).AddMinutes(get_minutes(workig_hours1.start.ToString("HH:mm")));
            }
            else
            {
                working_hours_merge.start = working_hours_merge.start.AddHours(get_hours(workig_hours2.start.ToString("HH:mm"))).AddMinutes(get_minutes(workig_hours2.start.ToString("HH:mm")));
            }
            if (workig_hours1.end.CompareTo(workig_hours2.end) < 0)
            {
                working_hours_merge.end = working_hours_merge.end.AddHours(get_hours(workig_hours1.end.ToString("HH:mm"))).AddMinutes(get_minutes(workig_hours1.end.ToString("HH:mm")));
            }
            else
            {
                working_hours_merge.end = working_hours_merge.end.AddHours(get_hours(workig_hours2.end.ToString("HH:mm"))).AddMinutes(get_minutes(workig_hours2.end.ToString("HH:mm")));
            }
            var merged_calendar = get_merged_calendar(planned_meeting1, planned_meeting2);
            var normalized_calendar = get_optimized_calendar(merged_calendar);
            var adjusted_calendar = get_working_hours(normalized_calendar, working_hours_merge);
            var availablse_slots = get_available_time_slots(adjusted_calendar, meeting_duration);
            //Wyjscie
            for (int i = 0; i < availablse_slots.start.Count; ++i)
            {
                Console.WriteLine(availablse_slots.start[i].ToString("HH:mm") + " - " + availablse_slots.end[i].ToString("HH:mm"));

            }
            Console.ReadLine();
        }
        public static int get_hours(string time)
        {
            string[] pom = time.Split(":");
            int hours = int.Parse(pom[0]);
            return Convert.ToInt32(hours);
        }
        public static int get_minutes(string time)
        {
            string[] pom = time.Split(":");
            int minutes = int.Parse(pom[1]);
            return Convert.ToInt32(minutes);
        }
        public static int get_minutes_value(string time)
        {
            string[] pom = time.Split(":");
            int hours = int.Parse(pom[0]);
            int minutes = int.Parse(pom[1]);
            return Convert.ToInt32(hours) * 60 + Convert.ToInt32(minutes);
        }
        public static int compare_times(string t1, string t2)
        {
            int p1 = get_minutes_value(t1);
            int p2 = get_minutes_value(t2);
            if (p1 >= p2)
            {
                return 1;
            }
            else if (p2 >= p1)
            {
                return -1;
            }
            return 0;
        }
        public static Calendar.planned_meeting get_merged_calendar(Calendar.planned_meeting planned_meeting1, Calendar.planned_meeting planned_meeting2)
        {
            Calendar.planned_meeting planned_meeting_merge = new Calendar.planned_meeting();
            var i = 0;
            var j = 0;
            while (i < planned_meeting1.start.Count && j < planned_meeting2.start.Count)
            {
                if (compare_times(planned_meeting1.start[i].ToString("HH:mm"), planned_meeting2.start[j].ToString("HH:mm")) == -1)
                {
                    planned_meeting_merge.start.Add(planned_meeting1.start[i]);
                    planned_meeting_merge.end.Add(planned_meeting1.end[i]);
                    i += 1;
                }
                else
                {
                    planned_meeting_merge.start.Add(planned_meeting2.start[j]);
                    planned_meeting_merge.end.Add(planned_meeting2.end[j]);
                    j += 1;
                }
            }
            while (i < planned_meeting1.start.Count)
            {
                planned_meeting_merge.start.Add(planned_meeting1.start[i]);
                planned_meeting_merge.end.Add(planned_meeting1.end[i]);
                i += 1;
            }
            while (j < planned_meeting2.start.Count)
            {
                planned_meeting_merge.start.Add(planned_meeting2.start[j]);
                planned_meeting_merge.end.Add(planned_meeting2.end[j]);
                j += 1;
            }
            return planned_meeting_merge;
        }
        public static Calendar.planned_meeting get_optimized_calendar(Calendar.planned_meeting merged_calendar)
        {
            Calendar.planned_meeting planned_meeting = new Calendar.planned_meeting();
            planned_meeting.start.Add(merged_calendar.start[0]);
            planned_meeting.end.Add(merged_calendar.end[0]);
            int i = 0;
            while (i < merged_calendar.start.Count)
            {
                var pom1_start = planned_meeting.start[planned_meeting.start.Count - 1];
                var pom1_end = planned_meeting.end[planned_meeting.end.Count - 1];
                planned_meeting.start.RemoveAt(planned_meeting.start.Count - 1);
                planned_meeting.end.RemoveAt(planned_meeting.end.Count - 1);
                var start_time1 = pom1_start;
                var end_time1 = pom1_end;
                var pom2_start = merged_calendar.start[i];
                var pom2_end = merged_calendar.end[i];
                var start_time2 = pom2_start;
                var end_time2 = pom2_end;
                if (compare_times(end_time1.ToString("HH:mm"), start_time2.ToString("HH:mm")) == 1)
                {
                    if (compare_times(end_time1.ToString("HH:mm"), end_time2.ToString("HH:mm")) == 1)
                    {
                        planned_meeting.start.Add(start_time1);
                        planned_meeting.end.Add(end_time1);
                    }
                    else
                    {
                        planned_meeting.start.Add(start_time1);
                        planned_meeting.end.Add(end_time2);
                    }
                }
                else
                {
                    planned_meeting.start.Add(start_time1);
                    planned_meeting.end.Add(end_time1);

                    planned_meeting.start.Add(merged_calendar.start[i]);
                    planned_meeting.end.Add(merged_calendar.end[i]);
                }
                i += 1;
            }
            return planned_meeting;
        }
        public static Calendar.planned_meeting get_available_time_slots(Calendar.planned_meeting original_calendar, int duration)
        {
            Calendar.planned_meeting planned_meeting = new Calendar.planned_meeting();
            int i = 0;
            while (i < original_calendar.start.Count - 1)
            {
                var end_time1 = original_calendar.end[i];
                var pom2_start = original_calendar.start[i + 1];
                var start_time2 = pom2_start;
                var p1 = get_minutes_value(end_time1.ToString("HH:mm"));
                var p2 = get_minutes_value(start_time2.ToString("HH:mm"));
                if (p1 != p2 && p2 - p1 >= duration)
                {
                    planned_meeting.start.Add(end_time1);
                    planned_meeting.end.Add(start_time2);
                }
                i += 1;
            }
            return planned_meeting;
        }
        public static Calendar.planned_meeting get_working_hours(Calendar.planned_meeting original_calendar, Calendar.working_hours working_hours)
        {
            var pom1_start = original_calendar.start[0];
            var start_time = pom1_start;
            var pom2_end = original_calendar.end[original_calendar.end.Count - 1];
            var end_time = pom2_end;
            if (compare_times(start_time.ToString("HH:mm"), working_hours.start.ToString("HH:mm")) == -1)
            {
                int i = 0;
                while (i < original_calendar.start.Count)
                {
                    if (compare_times(original_calendar.end[i].ToString("HH:mm"), working_hours.start.ToString("HH:mm")) == -1)
                    {
                        original_calendar.start.RemoveAt(i);
                        original_calendar.end.RemoveAt(i);
                    }
                    else i++;
                }
            }
            if (compare_times(end_time.ToString("HH:mm"), working_hours.end.ToString("HH:mm")) != -1)
            {
                int i = original_calendar.start.Count - 1;
                while (i >= 0)
                {
                    if (compare_times(original_calendar.start[i].ToString("HH:mm"), working_hours.end.ToString("HH:mm")) != -1)
                    {
                        original_calendar.end.RemoveAt(i);
                        original_calendar.start.RemoveAt(i);
                    }
                    i--;
                }
            }
            if (compare_times(original_calendar.start[0].ToString("HH:mm"), working_hours.start.ToString("HH:mm")) == -1)
            {
                original_calendar.start[0] = working_hours.start;

            }
            if (compare_times(end_time.ToString("HH:mm"), working_hours.end.ToString("HH:mm")) == -1)
            {
                original_calendar.start.Add(working_hours.end);
                original_calendar.end.Add(new DateTime().AddHours(23).AddMinutes(59));

                int i = original_calendar.start.Count - 1;
                while (i >= 1)
                {
                    if (compare_times(original_calendar.end[i].ToString("HH:mm"), original_calendar.start[i].ToString("HH:mm")) == -1)
                    {
                        original_calendar.end[i] = original_calendar.end[i - 1];
                    }
                    i--;
                }
            }
            else if (compare_times(original_calendar.end[original_calendar.start.Count - 1].ToString("HH:mm"), working_hours.start.ToString("HH:mm")) != -1)
            {
                original_calendar.end[original_calendar.start.Count - 1] = working_hours.end;
            }
            return original_calendar;
        }
    }
}

