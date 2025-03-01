using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAnimations : MonoBehaviour
{
    [SerializeField]
    private GameObject _bot;
    
    private BotController _botController;
    private Animator _animator;

    private void Start()
    {
        _botController = GetComponent<BotController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
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
        }
    }
}
