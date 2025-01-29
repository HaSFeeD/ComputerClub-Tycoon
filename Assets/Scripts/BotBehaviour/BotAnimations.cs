<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
using UnityEngine;

public class BotAnimations : MonoBehaviour
{
<<<<<<< HEAD
=======
    [SerializeField]
    private GameObject _bot;
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    private BotController _botController;
    private Animator _animator;

    private void Start()
    {
        _botController = GetComponent<BotController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
<<<<<<< HEAD
        if (_botController.isMovingToPoint)
        {
            _animator.SetBool("isWalking", true);
        }
        else if (_botController.isMovingToComputer)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }

        if (_botController.isWaitingAtPoint)
        {
            _animator.SetBool("isWaiting", true);
        }
        else
        {
            _animator.SetBool("isWaiting", false);
        }

        if (_botController.isUsingComputer)
        {
            _animator.SetBool("isUsingComputer", true);
        }
        else
        {
            _animator.SetBool("isUsingComputer", false);
=======
        switch (_botController.CurrentActivity)
        {
            case BotActivity.Walking:
                _bot.GetComponent<SkinnedMeshRenderer>().enabled = true; 
                _animator.SetBool("isWalking", true);
                _animator.SetBool("isWaiting", false);
                _animator.SetBool("isUsingComputer", false);
                _animator.SetBool("isUsingVendingMachine", false);
                _animator.SetBool("isUsingToilet", false);
                break;
            case BotActivity.Waiting:
                _bot.GetComponent<SkinnedMeshRenderer>().enabled = true; 
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isWaiting", true);
                _animator.SetBool("isUsingComputer", false);
                _animator.SetBool("isUsingVendingMachine", false);
                _animator.SetBool("isUsingToilet", false);
                break;
            case BotActivity.UsingComputer:
                _bot.GetComponent<SkinnedMeshRenderer>().enabled = true;      
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isWaiting", false);
                _animator.SetBool("isUsingComputer", true);
                _animator.SetBool("isUsingVendingMachine", false);
                _animator.SetBool("isUsingToilet", false);
                break;
            case BotActivity.UsingVendingMachine:
                _bot.GetComponent<SkinnedMeshRenderer>().enabled = true; 
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isWaiting", false);
                _animator.SetBool("isUsingComputer", false);
                _animator.SetBool("isUsingVendingMachine", true);
                _animator.SetBool("isUsingToilet", false);
                break;
            case BotActivity.UsingToilet:
                _bot.GetComponent<SkinnedMeshRenderer>().enabled = false;
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isWaiting", true);
                _animator.SetBool("isUsingComputer", false);
                _animator.SetBool("isUsingVendingMachine", false);
                _animator.SetBool("isUsingToilet", false);
                break;
            default:
                _bot.GetComponent<SkinnedMeshRenderer>().enabled = true; 
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isWaiting", false);
                _animator.SetBool("isUsingComputer", false);
                _animator.SetBool("isUsingVendingMachine", false);
                _animator.SetBool("isUsingToilet", false);
                break;
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
        }
    }
}
