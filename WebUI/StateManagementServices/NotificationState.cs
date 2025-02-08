using System.Net.Http.Json;
using Application.DTOs.Request.Entities;
using Application.Extension;
using Blazored.LocalStorage.StorageOptions;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebUI.StateManagementServices;

public class NotificationState
{
    private int _notificationsCount;
    private HubConnection? _hubConnection;
    private readonly HttpClient _httpClient;
    private readonly LocalStorageService _localStorageService;
    public int NotificationsCount
    {
        get => _notificationsCount;
        private set
        {
            _notificationsCount = value;
            OnNotificationCountChange?.Invoke();
        }
    }
    
    public NotificationState(HttpClient httpClient, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    public Action? OnNotificationCountChange { get; set; }

    public async Task InitializeAsync()
    {
        var tokenModel = await _localStorageService.GetModelFromToken();
        var accessToken = tokenModel?.Token;
        if (string.IsNullOrEmpty(accessToken)) return;

        var receiverId = await _localStorageService.GetUserIdFromToken();

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7201/chathub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(accessToken);
            })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<ThreadRequest>("ReceiveThreadRequest", async (request) =>
        {
            try
            {
                var threadRequests = await _httpClient.GetFromJsonAsync<List<ThreadRequestDto>>(
                    $"https://localhost:7201/api/chats/thread/receive/?ReceiverId={receiverId}");

                UpdateNotificationCount(threadRequests?.Count ?? 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching thread requests: {ex.Message}");
            }
        });

        await _hubConnection.StartAsync();

        // Initial fetch
        var initialThreadRequests = await _httpClient.GetFromJsonAsync<List<ThreadRequestDto>>(
            $"https://localhost:7201/api/chats/thread/receive/?ReceiverId={receiverId}");

        UpdateNotificationCount(initialThreadRequests?.Count ?? 0);
    }
    public void UpdateNotificationCount(int newNotificationCount)
    {
        NotificationsCount = newNotificationCount;
        OnNotificationCountChange?.Invoke();
    }
}