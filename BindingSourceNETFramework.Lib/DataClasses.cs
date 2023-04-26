using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingSourceNETFramework.Lib
{
    /// <summary>
    /// Bjj belts ranking enum
    /// </summary>
    public enum BjjBeltsEnum
    {
        No_Belt,
        White,
        Blue,
        Purple,
        Brown,
        Black,
        Red_Black,
        Red_White,
        Coral
    }
    /// <summary>
    /// Bjj wrestlers' countries enum
    /// </summary>
    public enum BjjCountries
    {
        No_Country,
        Russia,
        USA,
        Brasil,
        UK,
        Poland,
        Finland,
        Argentina
    }
    /// <summary>
    /// Bjj wrestlers' teams enum
    /// </summary>
    public enum BjjTeams
    {
        No_Team,
        TeamStrela,
        Ludus,
        FP,
        GFTEAM,
        CheckMat,
        ZR,
        Gracie_Barra,
        ATOS,
        Alliance,
        Al_Wahda,
        TeamNogueira
    }
    [Serializable]
    public class BjjWrestler
    {
        public int Id { get; set; }
        [DisplayName("Имя"), Category("Сводка")]
        public string FirstName { get; set; }
        [DisplayName("Фамилия"), Category("Сводка")]
        public string LastName { get; set; }
        [DisplayName("Победы"), Category("Рекорд")]
        [Description("После достижения черного пояса")]
        public int? Wins { get; set; }
        [DisplayName("Поражения"), Category("Рекорд")]
        [Description("После достижения черного пояса")]
        public int? Losses { get; set; }
        [DisplayName("Страна"), Category("Сводка")]
        public BjjCountries Country { get; set; }
        [DisplayName("Пояс"), Category("Сводка")]
        public BjjBeltsEnum Belt { get; set; }
        [DisplayName("Команда"), Category("Сводка")]
        public BjjTeams Team { get; set; }
        public string ImagePath { get; set; }

        public BjjWrestler(int id, string firstName, string lastName, 
                           int wins, int losses, BjjCountries country, 
                           BjjBeltsEnum belt, BjjTeams team, string imagePath) 
        { 
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Wins = wins;
            Losses = losses;
            Country = country;
            Belt = belt;
            Team = team;
            ImagePath = imagePath;
        }
        public BjjWrestler() 
        { 
            Id = -1;
            FirstName = default;
            LastName = default;
            Wins = default;
            Losses = default;
            Country = default;
            Belt = default;
            Team = default;
            ImagePath = default;
        }
        public string TeamToString
        {
            get
            {
                return Team.ToString();
            }
            set
            {
                Team.ToString();
            }
        }
    }
}
