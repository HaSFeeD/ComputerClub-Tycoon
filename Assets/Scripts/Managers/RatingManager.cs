using UnityEngine;
using System;

public class RatingManager : MonoBehaviour
{
    public static RatingManager Instance;

    public const float MaxClubRating = 10f;
    private const float MinClubRating = 0;

    private float _currentRating = 0;

    private float _ratingOnEachPCUpgrade;

    private float _ratingOfEachRoom;

    public const int UpgradeRatingFactor = 6;
    private const int roomRatingFactor = 3;


    public event Action<float> OnRatingChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("[RatingManager] Start calling InitializeRating()");
        if(_currentRating < MinClubRating){
            _currentRating = 0;
        }
    }

    public void InitializeRating()
    {
        Debug.Log("[RatingManager] InitializeRating() START");

        GetRatingOfEachUpgrade();
        GetRatingOfEachRoom();

        Debug.Log($"[RatingManager] InitializeRating() END: _currentRating = {_currentRating}");
    }

    public float GetCurrentRating()
    {
        return _currentRating;
    }
    public void SetRating(float amount){
        _currentRating = amount;
        Debug.Log(_currentRating);
        OnRatingChanged?.Invoke(_currentRating / MaxClubRating);
    }
    public void AddRating(float amount){
        _currentRating += amount;
        if (_currentRating > MaxClubRating)
            _currentRating = MaxClubRating;
        OnRatingChanged?.Invoke(_currentRating / MaxClubRating);
    }

    public float GetRatingOfEachUpgrade()
    {
        int totalMaxLevels = 0;
        foreach (var upgrade in UpgradeManager.Instance.AllUpgrades)
        {
            totalMaxLevels += upgrade._maxUpgradeLevel;
        }
        _ratingOnEachPCUpgrade = (float)UpgradeRatingFactor / totalMaxLevels;

        Debug.Log($"[RatingManager] GetRatingOfEachUpgrade(): _ratingOnEachPCUpgrade = {_ratingOnEachPCUpgrade}");
        return _ratingOnEachPCUpgrade;
    }

    public float GetRatingOfEachRoom()
    {
        _ratingOfEachRoom = (float)roomRatingFactor / RoomManager.Instance.AllRooms.Count;
        Debug.Log($"[RatingManager] GetRatingOfEachRoom(): _ratingOfEachRoom = {_ratingOfEachRoom}");
        return _ratingOfEachRoom;
    }

}
