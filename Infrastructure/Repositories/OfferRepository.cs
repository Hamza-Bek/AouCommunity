using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly AppDbContext _context;

    public OfferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralResponse> AddOfferAsync(Offer model, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
        
            if (user is null)
                return new GeneralResponse(false, "No user found to proceed this action");

           
        
            _context.Offers.Add(model);
            
            await _context.SaveChangesAsync();
        
            return new GeneralResponse(true, "Offer Submitted, Please wait for the admin approval");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(false , ex.Message);
        }
       
    }

    public async Task<GeneralResponse> RemoveOfferAsync(string offerId, string userId)
    {
        try
        {
            var offer = await _context.Offers.FirstOrDefaultAsync(i => i.Id == offerId);
        
            if(offer is null)
                return new GeneralResponse(false , "No offer found to proceed this action");
        
            if(offer.Author != userId)
                return new GeneralResponse(false , "You do not have access to this offer");

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
        
            return new GeneralResponse(true , "Offer removed successfully");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(false , ex.Message);
        }
    }

    public async Task<Offer> GetOfferById(string id)
    {
        try
        {
            var offer = await _context.Offers.Where(s => s.Status == EntityStatus.Approved).FirstOrDefaultAsync(i => i.Id == id);
            return offer;
        }
        catch (Exception ex)
        { 
            throw new (ex.Message);
        }
        
    }

    public async Task<IEnumerable<Offer>> GetAllOffersAsync()
    {
        var offers = await _context.Offers.Where(s => s.Status == EntityStatus.Approved).ToListAsync();
        return offers;
    }

    public async Task<IEnumerable<Offer>> GetPendingOffersAsync()
    {
        return await _context.Offers.Where(s => s.Status == EntityStatus.PendingApproval).ToListAsync();
    }

    public async Task<GeneralResponse> ApproveOfferAsync(string offerId)
    {
        var offer = await _context.Offers.FindAsync(offerId);
        if(offer is null)
            return new GeneralResponse(false , "No offer found to proceed this action");

        offer.Status = EntityStatus.Approved;

        _context.Offers.Update(offer);
        await _context.SaveChangesAsync();

        return new GeneralResponse(true, "Offer approved!");
    }

    public async Task<GeneralResponse> RejectOfferAsync(string offerId)
    {
        var offer = await _context.Offers.FindAsync(offerId);
        if(offer is null)
            return new GeneralResponse(false , "No offer found to proceed this action");

        offer.Status = EntityStatus.Rejected;

        _context.Offers.Update(offer);
        await _context.SaveChangesAsync();

        return new GeneralResponse(true, "Offer rejected!");
    }
}