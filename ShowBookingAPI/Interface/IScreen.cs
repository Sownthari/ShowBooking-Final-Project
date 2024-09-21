using ShowBooking.Models;
using System.Collections.Generic;

namespace ShowBooking.Interface
{
    public interface IScreen
    {
        Task CreateScreen(CreateScreenDTO screenDto);
        Task<IEnumerable<TheatreScreenDTO>> GetScreens(int theatreID);
        Task<IEnumerable<object>> GetScreensName(int theatreID);
        Task<CreateScreenDTO> GetScreenById(int screenID);
        Task UpdateScreen(CreateScreenDTO screenDto);
        Task DeleteScreen(int screenID);
        Task<IEnumerable<TicketType>> GetAllTicketTypes(int screenID);
        Task<TicketType> GetTicketTypeById(int id);
        Task CreateTicketType(TicketType ticketType);
        Task UpdateTicketType(TicketType ticketType);
        Task DeleteTicketType(int id);
        Task CreateScreenSeats(int screenID, List<RowConfig> rowConfigs);
        Task<IEnumerable<Seat>> GetSeatsByScreen(int screenID);
    }
}
