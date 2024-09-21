using ShowBooking.Interface;
using ShowBooking.Models;

namespace ShowBooking.Service
{
    public class ScreenService
    {
        private readonly IScreen _screenRepo;

        public ScreenService(IScreen screenRepo)
        {
            _screenRepo = screenRepo;
        }

        public Task CreateScreen(CreateScreenDTO screenDto)
        {
            return _screenRepo.CreateScreen(screenDto);
        }

        public Task CreateTicketType(TicketType ticketType)
        {
            return _screenRepo.CreateTicketType(ticketType);
        }

        public Task DeleteScreen(int screenID)
        {
            return _screenRepo.DeleteScreen(screenID);
        }

        public Task DeleteTicketType(int id)
        {
            return _screenRepo.DeleteTicketType(id);
        }

        public Task<IEnumerable<TicketType>> GetAllTicketTypes(int screenID)
        {
            return _screenRepo.GetAllTicketTypes(screenID);
        }

        public Task<CreateScreenDTO> GetScreenById(int screenID)
        {
            return _screenRepo.GetScreenById(screenID);
        }

        public Task<IEnumerable<TheatreScreenDTO>> GetScreens(int theatreID)
        {
            return _screenRepo.GetScreens(theatreID);
        }

        public Task<TicketType> GetTicketTypeById(int id)
        {
            return _screenRepo.GetTicketTypeById(id);
        }

        public Task UpdateScreen(CreateScreenDTO screenDto)
        {
            return _screenRepo.UpdateScreen(screenDto);
        }

        public Task UpdateTicketType(TicketType ticketType)
        {
            return _screenRepo.UpdateTicketType(ticketType);
        }

        public async Task CreateScreenSeats(int screenID, List<RowConfig> rowConfigs)
        {
            await _screenRepo.CreateScreenSeats(screenID, rowConfigs);
        }

        public async Task<IEnumerable<Seat>> GetSeatsByScreen(int screenID)
        {
            return await _screenRepo.GetSeatsByScreen(screenID);
        }

        public async Task<IEnumerable<object>> GetScreensName(int theatreID)
        {
            return await _screenRepo.GetScreensName(theatreID);
        }
    }
}
