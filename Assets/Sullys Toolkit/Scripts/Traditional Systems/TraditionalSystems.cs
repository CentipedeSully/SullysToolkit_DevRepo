using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    //Enums & Interfaces
    public enum GameboardLayer
    {
        Units,
        PointsOfInterest,
        Terrain
    }

    public interface IGamePiece
    {
        GameObject GetObject();

        Gameboard GetGameboard();

        void SetGameboard(Gameboard newGameboard);

        GameboardLayer GetGameboardLayer();

        void SetGameboardLayer(GameboardLayer newLayer);

        (int, int) GetGridPosition();

        void SetGridPosition(int x, int y);

    }

    public interface ICard
    {
        GameObject GetGameObject();

        CardDeck GetOriginDeck();

        void SetOriginDeck(CardDeck newDeck);


    }

    //Classes
    public class Gameboard
    {
        //Declarations
        private int _rows;
        private int _columns;
        private List<IGamePiece> _gamePiecesOnBoard;



        //Constructors




        //Interface Utils



        //Internal Utils



        //Getters, Setters, & Commands



    }

    public static class DieRoller
    {
        //Static Commands
        public static int RollDie(int numberOfSides)
        {
            if (numberOfSides > 0)
                return Random.Range(1, numberOfSides);
            else return 1;
        }
    }

    public class CardDeck
    {
        //Declarations
        private List<ICard> _decklist;
        private List<ICard> _currentCardsInDeck;




        //Constructors
        public CardDeck()
        {
            _decklist = new List<ICard>();
            _currentCardsInDeck = new List<ICard>();
        }




        //Getters, Setters, & Commands
        public int GetCurrentCardCount()
        {
            return _currentCardsInDeck.Count;
        }

        public int GetDecklistCount()
        {
            return _decklist.Count;
        }

        public bool IsCardInDecklist(ICard card)
        {
            return _decklist.Contains(card);
        }

        public bool IsCardCurrentlyInDeck(ICard card)
        {
            return _currentCardsInDeck.Contains(card);
        }

        public void AddNewCardToDecklist(ICard newCard)
        {
            if (!IsCardInDecklist(newCard))
            {
                _decklist.Add(newCard);
                _currentCardsInDeck.Insert(0,newCard);
            }
                
        }

        public void DeleteCardFromDecklist(ICard card)
        {
            if (IsCardInDecklist(card))
            {
                _currentCardsInDeck.Remove(card);
                _decklist.Remove(card);
            }
        }

        public void ReturnCardToDeck(ICard card, int returnLocation)
        {
            if (IsCardInDecklist(card) && !IsCardCurrentlyInDeck(card))
            {
                returnLocation = Mathf.Clamp(returnLocation, 0, _currentCardsInDeck.Count);
                _currentCardsInDeck.Insert(returnLocation, card);
            }
                
        }

        public void PullSpecificCardFromDeck(ICard card)
        {
            if (IsCardCurrentlyInDeck(card))
                _currentCardsInDeck.Remove(card);
        }

        public ICard PullCardFromLocationFromTop(int topOffset = 0)
        {
            if (GetCurrentCardCount() > 0)
            {
                topOffset = Mathf.Clamp(topOffset, 0, _currentCardsInDeck.Count - 1);
                int drawIndex = _currentCardsInDeck.Count - topOffset;

                ICard drawnCard = _currentCardsInDeck[drawIndex];
                _currentCardsInDeck.RemoveAt(drawIndex);
                return drawnCard;
            }
            else return null;


        }

        public void ShuffleDeck()
        {
            List<ICard> shuffledList = new List<ICard>();

            int shuffledCardCount = 0;
            int heldCardCount = _currentCardsInDeck.Count;
            while (shuffledCardCount < heldCardCount)
            {
                ICard randomCard = _currentCardsInDeck[Random.Range(0, _currentCardsInDeck.Count)];
                _currentCardsInDeck.Remove(randomCard);
                shuffledList.Add(randomCard);
                shuffledCardCount++;
            }

            _currentCardsInDeck = shuffledList;
        }

        public ICard InspectTopCardOfDeck(int topOffset = 0)
        {
            topOffset = Mathf.Clamp(topOffset, 0, _currentCardsInDeck.Count);
            return _currentCardsInDeck[_currentCardsInDeck.Count - topOffset];
        }

    }


}

