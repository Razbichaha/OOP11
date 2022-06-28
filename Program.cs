using System;
using System.Collections.Generic;
using System.Threading;

namespace OOP11
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramCore programCore = new();
            programCore.GameStart();
            Console.ReadLine();
        }
    }

    class ProgramCore
    {
        private Render _render = new Render();
        private List<Land> _lands = new List<Land>();

        internal void GameStart()
        {
            GenerateLand();
            ShowLands();
            _render.ShowMessageStartBatle();
            StartWar();
        }

        private void StartWar()
        {
            int attackLand = IndicateWhosFirst();
            bool continueGame = true;

            while (continueGame)
            {
                bool gameOver;

                _render.ShowBatle(_lands[0], _lands[1]);
                _lands[attackLand].Attack(_lands[Inventory(attackLand)], out gameOver);
                attackLand = Inventory(attackLand);

                if (gameOver == true)
                    continueGame = false;

                Thread.Sleep(100);
            }
            NominateWinner();
        }

        private void NominateWinner()
        {
            foreach (Land land in _lands)
            {
                if (land.GetQuantitySoldiers() != 0)
                {
                    _render.ShowWinner(land);
                }
            }
        }

        private int Inventory(int number)
        {
            int numberTemp = 0;

            if (number == 0)
            {
                numberTemp = 1;
            }
            return numberTemp;
        }

        private int IndicateWhosFirst()
        {
            Random random = new Random();
            int first = random.Next(0, _lands.Count - 1);

            return first;
        }

        private void ShowLands()
        {
            foreach (Land land in _lands)
            {
                _render.ShowLand(land);
            }
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

    class Render
    {
        public Render()
        {
            Console.WindowHeight = 40;
        }

        internal void ShowWinner(Land land)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Победитель");
            Console.ForegroundColor = ConsoleColor.Yellow;

            ShowLand(land);
        }

        internal void ShowLand(Land land)
        {
            Console.WriteLine($"Страна - {land._number + 1}");
            Console.WriteLine("Вооруженные силы в своем составе имеют.");
            Console.WriteLine($"Взводов - {land.GetQuantityPlatoons()}");
            Console.WriteLine($"Отрядов входящих во взвод - {land.GetQuantitySquads()}.");
            Console.WriteLine($"Общей численностью - {land.GetQuantitySoldiers()} солдат.");
            Console.WriteLine();
        }

        internal void ShowMessageStartBatle()
        {
            Console.WriteLine("______________________________________");
            Console.WriteLine("Хотите начать войну?\nНажмите Enter.");
            Console.ReadLine();
        }

        internal void ShowBatle(Land land1, Land land2)
        {
            int cursorPositionLand1Left = 1;
            int cursorPositionLand1Top = 1;
            int cursorOffset = 50;
            int cursorPositionLand2Left = cursorPositionLand1Left + cursorOffset;
            int cursorPositionLand2Top = 1;

            Console.Clear();
            Console.WriteLine($"Страна - {land1._number + 1}");
            ShowPlatoon(land1.GetPlatoons(), cursorPositionLand1Left, cursorPositionLand1Top);
            Console.SetCursorPosition(cursorPositionLand2Left, cursorPositionLand2Top - 1);
            Console.WriteLine($"Страна - {land2._number + 1}");
            ShowPlatoon(land2.GetPlatoons(), cursorPositionLand2Left, cursorPositionLand2Top);
        }

        private void ShowPlatoon(List<Platoon> platoons, int cursorPositionLeft, int cursorPositionTop)
        {
            foreach (Platoon platoon in platoons)
            {
                Console.SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                Console.Write($"Взвод - {platoon._id + 1}  Отрядов {platoon.GetQuantitySquad()}  ");
                ShowSqud(platoon.GetSquad(), cursorPositionLeft, cursorPositionTop + 1);
                cursorPositionTop += platoon.GetQuantitySquad();
            }
        }

        private void ShowSqud(List<Squad> squads, int cursorPositionLeft, int cursorPositionTop)
        {
            int counter = 1;

            foreach (Squad squad in squads)
            {
                Console.SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Отряд - {counter}  Бойцов-{squad.GetQuantity()}   \n");
                Console.ForegroundColor = ConsoleColor.White;
                counter++;
                ShowSoldier(squad.GetSoldiers(), cursorPositionLeft, cursorPositionTop + 1);
                cursorPositionTop += squad.GetQuantity() + 1;
            }
        }

        private void ShowSoldier(List<Soldier> soldiers, int cursorPositionLeft, int cursorPositionTop)
        {
            int counter = 1;

            foreach (Soldier soldier in soldiers)
            {
                int life = soldier.GetLife();
                Console.SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                Console.Write($"  Боец - {counter}  Жизнь-{life}\n");
                cursorPositionTop++;
                counter++;

                if (life == 0)
                {
                    Console.Clear();
                }
            }
        }
    }

    #region отряды
    class Land
    {
        private int _quantityPlaatons = 1;
        private List<Platoon> _platoons = new List<Platoon>();
        internal int _number { get; private set; }

        internal Land(int number)
        {
            _number = number;
            GenerateListPlatoons();
        }

        internal void Attack(Land land, out bool gameOver)
        {
            gameOver = false;
            int[] idPlatoonsDefense = GetIdPlatoons(land);
            int counter = 0;

            for (int i = 0; i < _platoons.Count; i++)
            {
                if (idPlatoonsDefense.Length != 0)
                {
                    _platoons[i].Attack(_platoons[i], land._platoons[counter]);
                    GetEnumerationIdPlatoonsDefense(idPlatoonsDefense.Length, ref counter);
                }
            }

            foreach (Platoon platoon in _platoons)
            {
                if (platoon.GetQuantitySquad() == 0)
                {
                    gameOver = true;
                }
            }
        }

        internal List<Platoon> GetPlatoons()
        {
            List<Platoon> platoons = new List<Platoon>();

            foreach (Platoon platoon in _platoons)
            {
                platoons.Add(platoon);
            }
            return platoons;
        }

        internal int GetQuantityPlatoons()
        {
            return _platoons.Count;
        }

        internal int GetQuantitySquads()
        {
            int quantity = 0;

            foreach (Platoon platoon in _platoons)
            {
                quantity += platoon.GetQuantitySquad();
            }
            return quantity;
        }

        internal int GetQuantitySoldiers()
        {
            int quantity = 0;

            foreach (Platoon platoon in _platoons)
            {
                quantity += platoon.GetQuantitySoldiersSquads();
            }
            return quantity;
        }

        private int[] GetIdPlatoons(Land land)
        {
            int[] idPlatoonsDefense = new int[land._platoons.Count];

            for (int i = 0; i < land._platoons.Count; i++)
            {
                idPlatoonsDefense[i] = land._platoons[i]._id;
            }
            return idPlatoonsDefense;
        }

        private void GetEnumerationIdPlatoonsDefense(int lengthMassiv, ref int counter)
        {
            counter++;

            if (counter >= lengthMassiv)
            {
                counter = 0;
            }
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

    class Platoon
    {
        private int _maximumNumberSquads = 5;
        private int _minimumNumberSquads = 3;
        private List<Squad> _squads = new List<Squad>();

        internal int _id { get; private set; }

        internal Platoon()
        {
            Generate();
        }

        internal void Attack(Platoon attac, Platoon defense)
        {
            int[] idPlatoonsDefense = GetIdSquads(defense);
            int counter = 0;
            List<Squad> deleteSquad = new List<Squad>();

            for (int i = 0; i < attac._squads.Count; i++)
            {
                int count = defense._squads.Count;

                if (count != 0)
                {
                    attac._squads[i].Attack(defense._squads[counter]);
                    GetEnumerationIdplatoonsDefense(idPlatoonsDefense.Length, ref counter);
                }
            }

            foreach (Squad squad in defense._squads)
            {
                if (squad.GetQuantity() == 0)
                {
                    deleteSquad.Add(squad);
                }
            }

            foreach (Squad squad in deleteSquad)
            {
                defense._squads.Remove(squad);
            }
        }

        internal List<Squad> GetSquad()
        {
            List<Squad> squads = new List<Squad>();

            foreach (Squad squad in _squads)
            {
                squads.Add(squad);
            }
            return squads;
        }

        internal int GetQuantitySquad()
        {
            return _squads.Count;
        }

        internal int GetQuantitySoldiersSquads()
        {
            int quantity = 0;

            foreach (Squad squad in _squads)
            {
                quantity += squad.GetQuantity();
            }
            return quantity;
        }

        private void Generate()
        {
            Random random = new Random();

            for (int i = 0; i < random.Next(_minimumNumberSquads, _maximumNumberSquads); i++)
            {
                Squad squad = new Squad();
                _squads.Add(squad);
            }
        }

        private int[] GetIdSquads(Platoon platoon)
        {
            int[] idPlatoonsDefense = new int[platoon._squads.Count];

            for (int i = 0; i < platoon._squads.Count; i++)
            {
                idPlatoonsDefense[i] = i;
            }
            return idPlatoonsDefense;
        }

        private void GetEnumerationIdplatoonsDefense(int lengthMassiv, ref int counter)
        {
            counter++;

            if (counter >= lengthMassiv)
            {
                counter = 0;
            }
        }
    }

    class Squad
    {
        private int _maximumSoldier = 10;
        private int _minimumSoldier = 5;
        private int _quantityMachineGunner = 1;
        private List<Soldier> _soldiers = new List<Soldier>();

        internal Squad()
        {
            Generate();
        }

        internal void Attack(Squad defense)
        {
            List<Soldier> deleteSoldiers = new List<Soldier>();
            int counter = 0;

            for (int i = 0; i < _soldiers.Count; i++)
            {
                if (defense._soldiers.Count != 0)
                {
                    Soldier defense1 = defense._soldiers[counter];
                    Soldier defense2 = defense._soldiers[counter];
                    GetEnumerationSoldierDefense(defense._soldiers, ref counter);

                    int damage = _soldiers[i].Damage;
                    _soldiers[i].Fair(damage, defense1, defense2);
                }
            }

            foreach (Soldier soldier in defense._soldiers)
            {
                if (soldier.GetLife() == 0)
                {
                    deleteSoldiers.Add(soldier);
                }
            }

            for (int i = 0; i < deleteSoldiers.Count; i++)
            {
                defense._soldiers.Remove(deleteSoldiers[i]);
            }
        }

        internal int GetQuantity()
        {
            return _soldiers.Count;
        }

        internal List<Soldier> GetSoldiers()
        {
            List<Soldier> soldiers = new List<Soldier>();

            foreach (Soldier soldier in _soldiers)
            {
                soldiers.Add(soldier);
            }
            return soldiers;
        }

        private void GetEnumerationSoldierDefense(List<Soldier> soldiers, ref int counter)
        {
            counter++;

            if (counter >= soldiers.Count - 1)
            {
                counter = 0;
            }
        }

        private void Generate()
        {
            Random random = new Random();

            for (int i = 0; i < _quantityMachineGunner; i++)
            {
                MachineGunner machineGun = new MachineGunner();
                _soldiers.Add(machineGun);
            }

            for (int i = 0; i < random.Next(_minimumSoldier, _maximumSoldier) - _quantityMachineGunner; i++)
            {
                Shooter shooter = new Shooter();
                _soldiers.Add(shooter);
            }
        }
    }
    #endregion

    #region солдаты
    class Soldier
    {
        private int Life;

        internal virtual int Damage { get; }

        internal virtual int GetLife()
        {
            return Life;
        }

        internal virtual void SetLife(int damage)
        {
        }

        virtual internal void Fair(int damage, Soldier defender1, Soldier defender2)
        { }
    }

    class MachineGunner : Soldier
    {
        internal string speciality = "Пулеметчик";

        private int Life;

        internal override int Damage => 10;

        public MachineGunner()
        {
            Life = 100;
        }

        internal override int GetLife()
        {
            return Life;
        }

        internal override void Fair(int damage, Soldier defender1, Soldier defender2)
        {
            int damageMachinGun = Convert.ToInt32(damage * 0.6);
            defender1.SetLife(damageMachinGun);
            defender2.SetLife(damageMachinGun);
        }

        internal override void SetLife(int damage)
        {
            Life -= damage;
            if (Life < 0)
            {
                Life = 0;
            }
        }
    }

    class Shooter : Soldier
    {
        internal string speciality = "Стрелок";
        private int Life;

        internal override int Damage => 15;

        public Shooter()
        {
            Life = 120;
        }

        internal override int GetLife()
        {
            return Life;
        }

        internal override void Fair(int damage, Soldier defender, Soldier defender2)
        {
            defender.SetLife(damage);
        }

        internal override void SetLife(int damage)
        {
            Life -= damage;
            if (Life < 0)
            {
                Life = 0;
            }
        }
    }
    #endregion
}
