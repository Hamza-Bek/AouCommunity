using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Application.Validators.Entities;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OffersController : ControllerBase
{
    private readonly IOfferRepository _offerRepository;
    private readonly IValidator<OfferDto> _offerValidator;
    
    public OffersController(IOfferRepository offerRepository, IValidator<OfferDto> offerValidator)
    {
        _offerRepository = offerRepository;
        _offerValidator = offerValidator;
    }

    [HttpPost("create/offer")]
    public async Task<IActionResult> AddOfferAsync(OfferDto model, string userId)
    {
        var validationResult = await _offerValidator.ValidateAsync(model);
        
        if(!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var offer = new Offer()
        {
            Id = Guid.NewGuid().ToString(),
            Title = model.Title,
            Content = model.Content,
            Author = userId,
            Price = model.Price,
            CreatedDate = DateTime.UtcNow,
            Status = EntityStatus.PendingApproval
        };
        
        var result = await _offerRepository.AddOfferAsync(offer, userId);
        return Ok(result);
    }

    [HttpDelete("delete/offer/{offerId}/{userId}")]
    public async Task<IActionResult> DeleteOfferAsync(string offerId, string userId)
    {
        var result = await _offerRepository.RemoveOfferAsync(offerId, userId);
        return Ok();
    }

    [HttpGet("get/offer/{offerId}")]
    public async Task<IActionResult> GetOfferById(string offerId)
    {
        var result = await _offerRepository.GetOfferById(offerId);
        return Ok(result);
        
    }

    [HttpGet("get/offers")]
    public async Task<IActionResult> GetAllOffersAsync()
    {
        var result = await _offerRepository.GetAllOffersAsync();
        return Ok(result);
    }

    [HttpGet("get/pending-offers")]
    public async Task<IActionResult> GetPendingOffersAsync()
    {
        var result = await _offerRepository.GetPendingOffersAsync();
        return Ok(result);
    }

    [HttpPost("approve/offer")]
    public async Task<IActionResult> ApproveOfferAsync(string offerId)
    {
        var result = await _offerRepository.ApproveOfferAsync(offerId);
        return Ok(result);
    }

    [HttpPost("reject/offer")]
    public async Task<IActionResult> RejectOfferAsync(string offerId)
    {
        var result = await _offerRepository.RejectOfferAsync(offerId);
        return Ok(result);
    }
}