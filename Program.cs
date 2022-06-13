using System;
using System.Collections.Generic;

namespace OOP11
{
    //    Есть 2 взвода. 1 взвод страны один, 2 взвод страны два.
    //Каждый взвод внутри имеет солдат.
    //Нужно написать программу, которая будет моделировать бой этих взводов.
    //Каждый боец - это уникальная единица,
    //он может иметь уникальные способности или же уникальные характеристики, такие как повышенная сила.
    //Побеждает та страна, во взводе которой остались выжившие бойцы.
    //Не важно, какой будет бой, рукопашный, стрелковый.
    class Program
    {
        static void Main(string[] args)
        {
            ProgramCore programCore = new();
            programCore.GameStart();

            Console.ReadLine();/////////////
        }
    }

    class ProgramCore
    {

        private List<Land> _lands = new List<Land>();

        internal void GameStart()
        {
            GenerateLand();


        }

        private void GenerateLand()
        {
            int quantityLand = 2;

            for (int i = 0; i < quantityLand; i++)
            {
                Land land = new Land(i);
                _lands.Add(land);
            }
        }

    }
    #region отряды
    class Land //противоборствующая сторона, входит 1 Plaaton, стран 2
    {
        private int _quantityPlaatons = 1;
        internal int _number { get; private set; }

        private List<Platoon> _platoons = new List<Platoon>();


        internal Land(int number)
        {
            _number = number;
            GenerateListPlatoons();


        }

        private void GenerateListPlatoons()
        {
            for (int i = 0; i < _quantityPlaatons; i++)
            {
                Platoon platoon = new Platoon();
                _platoons.Add(platoon);
            }
        }

    }

    class Platoon //15-60 бойцов или 3-5 Squad
    {
        private int _maximumNumberSquads = 5;
        private int _minimumNumberSquads = 3;

        internal int _number { get; private set; }

        private List<Squad> _squads = new List<Squad>();

       internal Platoon()
        {
            Generate();

        }

        private void Generate()
        {
            Random random = new Random();
            for (int i = 0; i < random.Next(_minimumNumberSquads,_maximumNumberSquads); i++)
            {
                Squad squad = new Squad();
                _squads.Add(squad);
            }
        }

    }

    class Squad //5- 10 бойцов
    {
        private int _maximumSoldier = 10;
        private int _minimumSoldier = 5;

        private int _quantityMachineGunner = 1;






    }
    #endregion

    #region солдаты
    abstract class Soldier//боец
    {
        internal int Id { get; private set; }


    }

    class MachineGunner:Soldier//пулеметчик
    {


    }

    class Shooter:Soldier//стрелок
    {


    }
    #endregion

    #region оружие
    abstract class Weapon
    {
        internal int Damage { get; private set; }

       virtual internal void Fair()
        {

        }
    }

    class MachineGun:Weapon
    {


    }

    class Gun:Weapon
    {


    }

    #endregion
}
