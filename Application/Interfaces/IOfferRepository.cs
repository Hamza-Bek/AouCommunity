using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces
{
  public interface IOfferRepository
  {
    Task<GeneralResponse> AddOfferAsync(Offer model , string userId);
    Task<GeneralResponse> RemoveOfferAsync(string offerId , string userId);
    Task<Offer> GetOfferById(string id);
    Task<IEnumerable<Offer>> GetAllOffersAsync();
    
     
    Task<IEnumerable<Offer>> GetPendingOffersAsync();
    Task<GeneralResponse> ApproveOfferAsync(string offerId);
    Task<GeneralResponse> RejectOfferAsync(string offerId);
  }
}