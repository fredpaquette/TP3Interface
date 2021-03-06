﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Event_Type { get; set; }
        public string Description { get; set; }
        private DateTime _start;
        public DateTime Starting { get { return _start; } set { _start = RoundToMinutes(value, 5); } }
        private DateTime _End;
        public DateTime Ending { get { return _End; } set { _End = RoundToMinutes(value, 5); } }
        public System.Windows.Forms.Panel ParentPanel { get; set; }

        public Event()
        {
            Starting = DateTime.Now;
            Ending = DateTime.Now;
            Event_Type = 0;
            
            
        }
        public Event(string Id, string Title, string Description, DateTime Starting, DateTime Ending,int Event_Type=0)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;;
            this.Starting = Starting;
            this.Ending = Ending;
            this.Event_Type = Event_Type;
        }
        public Event(string Id, string Title, string Description, string Starting, string Ending,string Event_Type)
        {
            this.Id = Id;
            this.Title = TextFilter.FromSql(Title);
            this.Description = TextFilter.FromSql(Description);
            this.Starting = DateTime.Parse(Starting);
            this.Ending = DateTime.Parse(Ending);
            if(Event_Type!="")
                this.Event_Type = int.Parse(Event_Type);
        }

        public Event(Event copy)
        {
            Id = copy.Id;
            Title = copy.Title;
            Description = copy.Description;
            Starting = copy.Starting;
            Ending = copy.Ending;
            ParentPanel = copy.ParentPanel;
            Event_Type = copy.Event_Type;
        }
        public Event Klone()
        {
            Event klone = new Event(this);
            return klone;
        }

        public static DateTime RoundToMinutes(DateTime date, int minutes)
        {
            float totaMinutes = date.Hour * 60 + date.Minute;
            int roundedMinutes = (int)Math.Truncate((totaMinutes + minutes / 2f) / minutes) * minutes;
            int hours = (int)Math.Truncate(roundedMinutes / 60F);
            roundedMinutes = roundedMinutes - hours * 60;
            return new DateTime(date.Year, date.Month, date.Day, hours, roundedMinutes, 0);
        }

        public static int PixelToMinutes(int Pixel, int Height)
        {
            float totalMinutesInDay = 24F * 60F;
            float minutes = (Pixel / (float)Height) * totalMinutesInDay;
            return (int)minutes;
        }
          
        public static int HourToPixel(int Hour, int Minute, int Height)
        {
            float minutes = Hour * 60F + Minute;
            float totalMinutesInDay = 24F * 60F;
            return (int)Math.Round((minutes / totalMinutesInDay) * Height);
        }
        public static int HourToPixel(DateTime date, int Height)
        {
            return HourToPixel(date.Hour, date.Minute, Height);
        }

        public static int DayOfWeekToPixel(int dayNum, int Width)
        {
            return (int)Math.Round(dayNum / 7F * Width);
        }

        public static int DayOfWeekToPixel(DateTime date, int Width)
        {
            return DayOfWeekToPixel((int)date.DayOfWeek, Width);
        }

        public Rectangle GetBorder()
        {
            if (ParentPanel != null)
            {
                Point Location = new Point(DayOfWeekToPixel(Starting, ParentPanel.Width)+1, HourToPixel(Starting, ParentPanel.Height));
                int Height = HourToPixel(Ending, ParentPanel.Height) - HourToPixel(Starting, ParentPanel.Height);
                int width = (int)Math.Round(ParentPanel.Width / 7F);

                Rectangle border = new Rectangle(Location, new Size(width-2, Height));
                return border;
            }
            return new Rectangle();
        }

        public static string TwoDigits(int n)
        {
            return (n < 10 ? "0" : "") + n.ToString();
        }
        public static string TimeToString(DateTime date)
        {
            return TwoDigits(date.Hour) + ":" + TwoDigits(date.Minute);
        }
        public void Draw(Graphics DC)
        {
            ;
            DC.FillRectangle(new SolidBrush(Color.FromArgb(Int32.Parse( Properties.Settings.Default.Event_Type_Colors[Event_Type].Split(',').ElementAt(0)), Int32.Parse(Properties.Settings.Default.Event_Type_Colors[Event_Type].Split(',').ElementAt(1)), Int32.Parse(Properties.Settings.Default.Event_Type_Colors[Event_Type].Split(',').ElementAt(2)))), GetBorder());
            DC.DrawRectangle(new Pen(Color.Black, 1), GetBorder());
            string time = TimeToString(Starting) + "-" + TimeToString(Ending);


            int fontSize = 8;
            using (Font font = new Font(Properties.Settings.Default.CalendarFontDesc.Name, fontSize - 1, FontStyle.Regular, GraphicsUnit.Point))
            {
                using (Font font2 = new Font(Properties.Settings.Default.CalendarFontTitre.Name, fontSize + 1, FontStyle.Bold, GraphicsUnit.Point))
                {
                    System.Windows.Forms.TextFormatFlags flags = System.Windows.Forms.TextFormatFlags.WordEllipsis | System.Windows.Forms.TextFormatFlags.HorizontalCenter;
                    Rectangle border = GetBorder();
                    fontSize += 6;

                    System.Windows.Forms.TextRenderer.DrawText(DC, time, font, border, Properties.Settings.Default.CalendarFontColorDesc, flags);
                    border = new Rectangle(border.Location.X, border.Location.Y + fontSize, border.Width, border.Height - fontSize);

                    System.Windows.Forms.TextRenderer.DrawText(DC, Title, font2, border, Properties.Settings.Default.CalendarFontColorTitre, flags);
                    border = new Rectangle(border.Location.X, border.Location.Y + fontSize, border.Width, border.Height - fontSize);

                    flags = System.Windows.Forms.TextFormatFlags.WordBreak;
                    System.Windows.Forms.TextRenderer.DrawText(DC, Description, font, border, Properties.Settings.Default.CalendarFontColorDesc, flags);
                }
            }
        }
    }
}
