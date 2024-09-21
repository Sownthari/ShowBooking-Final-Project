using Microsoft.EntityFrameworkCore;
using ShowBooking.Models;
using ShowBooking.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowBooking.Repository
{
    public class ScreenRepository : IScreen
    {
        private readonly ShowContext _context;

        public ScreenRepository(ShowContext context)
        {
            _context = context;
        }

        public async Task CreateScreen(CreateScreenDTO screenDto)
        {
            var screen = new TheatreScreen
            {
                TheatreID = screenDto.TheatreID,
                ScreenName = screenDto.ScreenName,
                SeatingCapacity = screenDto.SeatingCapacity,
                ScreenType = screenDto.ScreenType
            };

            _context.TheatreScreens.Add(screen);
            await _context.SaveChangesAsync();

            var ticketTypes = new List<TicketType>();

            foreach (var ticketTypeDto in screenDto.TicketTypes)
            {
                var ticketType = new TicketType
                {
                    TicketTypeName = ticketTypeDto.TicketTypeName,
                    Price = ticketTypeDto.Price,
                    ScreenID = screen.ScreenID
                };

                ticketTypes.Add(ticketType);
            }

            _context.TicketTypes.AddRange(ticketTypes);
            await _context.SaveChangesAsync(); 

            
            List<Seat> seats = new List<Seat>();

            foreach (var seatDto in screenDto.Seats)
            {
                var ticketType = ticketTypes.FirstOrDefault(t => t.TicketTypeName == seatDto.TicketType);

                if (ticketType != null)
                {
                    for (int columnIndex = 1; columnIndex <= seatDto.SeatColumn; columnIndex++)
                    {
                        var seat = new Seat
                        {
                            ScreenID = screen.ScreenID,
                            SeatNumber = $"{seatDto.SeatRow}{columnIndex}",
                            SeatRow = seatDto.SeatRow,
                            SeatColumn = columnIndex,
                            TicketTypeID = ticketType.TicketTypeID,
                            IsActive = true
                        };

                        seats.Add(seat);
                    }
                }
            }


            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
        } //create screen

        public async Task<IEnumerable<TheatreScreenDTO>> GetScreens(int theatreID)
        {
            return await _context.TheatreScreens
                .Where(screen => screen.TheatreID == theatreID)
                .Select(screen => new TheatreScreenDTO
                {
                    ScreenID = screen.ScreenID,
                    ScreenName = screen.ScreenName,
                    ScreenCapacity = screen.SeatingCapacity,
                    ScreenType = screen.ScreenType,
                    TicketTypes = screen.TicketTypes.Select(tt => new TicketTypeDTO
                    {
                        TicketTypeID = tt.TicketTypeID,
                        TicketTypeName = tt.TicketTypeName,
                        Price = tt.Price,
                        SeatCount = tt.Seats.Count(s => s.TicketTypeID == tt.TicketTypeID)
                    }).ToList()
                })
                .ToListAsync();
        }//getbytheatreadmin


        public async Task<CreateScreenDTO> GetScreenById(int screenID)
        {
            var screen = await _context.TheatreScreens
                .Include(t => t.TicketTypes)
                    .ThenInclude(tt => tt.Seats)
                .FirstOrDefaultAsync(ts => ts.ScreenID == screenID);

            if (screen == null)
            {
                return null;
            }

            var screenDto = new CreateScreenDTO
            {
                TheatreID = screen.TheatreID,
                ScreenID = screen.ScreenID,
                ScreenName = screen.ScreenName,
                SeatingCapacity = screen.SeatingCapacity,
                ScreenType = screen.ScreenType,
                TicketTypes = screen.TicketTypes.Select(tt => new TicketDTO
                {
                    TicketTypeID = tt.TicketTypeID,
                    TicketTypeName = tt.TicketTypeName,
                    Price = tt.Price
                }).ToList(),

                Seats = screen.TicketTypes.SelectMany(tt => tt.Seats)
                .GroupBy(s => new { s.SeatRow, s.TicketType.TicketTypeName })
                .Select(group => new SeatDTO
                {
                    SeatRow = group.Key.SeatRow,
                    SeatColumn = group.Count(),
                    TicketType = group.Key.TicketTypeName
                }).ToList()
            };

            return screenDto;
        }//getbyscreenadmin

        public async Task<IEnumerable<object>> GetScreensName(int theatreID)
        {
            var screens = await _context.TheatreScreens
                .Where(s => s.TheatreID == theatreID)
                .Select(s => new
                {
                    ScreenID = s.ScreenID,
                    ScreenName = s.ScreenName
                })
                .ToListAsync();

            return screens;
        }

        public async Task UpdateScreen(CreateScreenDTO screenDto) //updatescreen,seats,tickettypes of screen
        {

            var existingScreen = await _context.TheatreScreens
                .Include(s => s.TicketTypes)
                .ThenInclude(tt => tt.Seats)
                .FirstOrDefaultAsync(s => s.ScreenID == screenDto.ScreenID);

            if (existingScreen == null)
            {
                throw new Exception("Screen not found.");
            }

            existingScreen.ScreenName = screenDto.ScreenName;
            existingScreen.ScreenType = screenDto.ScreenType;
            existingScreen.SeatingCapacity = screenDto.SeatingCapacity;


            foreach (var ticketDto in screenDto.TicketTypes)
            {
                var existingTicketType = existingScreen.TicketTypes
                    .FirstOrDefault(tt => tt.TicketTypeName == ticketDto.TicketTypeName && tt.ScreenID == screenDto.ScreenID);

                if (existingTicketType != null)
                {
                    existingTicketType.Price = ticketDto.Price;
                }
                else
                {
                    var newTicketType = new TicketType
                    {
                        TicketTypeName = ticketDto.TicketTypeName,
                        Price = ticketDto.Price,
                        ScreenID = screenDto.ScreenID
                    };
                    existingScreen.TicketTypes.Add(newTicketType);
                }
            }

            var removedTicketTypes = existingScreen.TicketTypes
                .Where(tt => !screenDto.TicketTypes.Any(dto => dto.TicketTypeName == tt.TicketTypeName && tt.ScreenID == screenDto.ScreenID))
                .ToList();
            foreach (var removedTicketType in removedTicketTypes)
            {
                _context.Seats.RemoveRange(removedTicketType.Seats);
                _context.TicketTypes.Remove(removedTicketType);
            }

            await _context.SaveChangesAsync();

            foreach (var seatDto in screenDto.Seats)
            {
                var ticketType = _context.TicketTypes.FirstOrDefault(t => t.TicketTypeName == seatDto.TicketType && t.ScreenID == screenDto.ScreenID);

                if (ticketType == null)
                {
                    throw new Exception($"Ticket type {seatDto.TicketType} not found.");
                }

                var existingSeatsInRow = _context.Seats
                    .Where(s => s.SeatRow == seatDto.SeatRow && s.ScreenID == screenDto.ScreenID)
                    .ToList();
                if (existingSeatsInRow.Count > 0)
                {
                    var existingTicketType = existingSeatsInRow.FirstOrDefault().TicketType.TicketTypeName;
                    var existingTicketId = existingSeatsInRow.FirstOrDefault().TicketType.TicketTypeID;
                    
                    if (existingTicketType != ticketType.TicketTypeName)
                    {
                        foreach (var seat in existingSeatsInRow)
                        {
                            seat.TicketTypeID = ticketType.TicketTypeID;
                        }
                    }
                }

                var maxColumn = existingSeatsInRow.Any() ? existingSeatsInRow.Max(s => s.SeatColumn) : 0;

                if (seatDto.SeatColumn > maxColumn)
                {
                    for (int columnIndex = maxColumn + 1; columnIndex <= seatDto.SeatColumn; columnIndex++)
                    {
                        var newSeat = new Seat
                        {
                            ScreenID = screenDto.ScreenID,
                            SeatRow = seatDto.SeatRow,
                            SeatColumn = columnIndex,
                            SeatNumber = $"{seatDto.SeatRow}{columnIndex}",
                            TicketTypeID = ticketType.TicketTypeID,
                            IsActive = true
                        };
                        _context.Seats.Add(newSeat);
                    }
                }

                else if (seatDto.SeatColumn < maxColumn)
                {
                    var seatsToRemove = existingSeatsInRow
                        .Where(s => s.SeatColumn > seatDto.SeatColumn)
                        .ToList();

                    foreach (var seatToRemove in seatsToRemove)
                    {
                        _context.Seats.Remove(seatToRemove);
                    }
                }
            }
            var removedRows = existingScreen.TicketTypes
                    .SelectMany(tt => tt.Seats)
                    .Where(s => !screenDto.Seats.Any(dto => dto.SeatRow == s.SeatRow && s.ScreenID == screenDto.ScreenID))
                    .GroupBy(s => s.SeatRow)
                    .ToList();

            if(removedRows.Count > 0)
            {
                foreach (var removedRow in removedRows)
                {
                    var seatsToRemove = removedRow.ToList();
                    _context.Seats.RemoveRange(seatsToRemove);
                }
            }

            await _context.SaveChangesAsync();

        }

        public async Task DeleteScreen(int screenID)
        {
            var screen = await _context.TheatreScreens.FindAsync(screenID);
            if (screen != null)
            {
                _context.TheatreScreens.Remove(screen);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Screen with ID {screenID} not found.");
            }
        }

        public async Task<IEnumerable<TicketType>> GetAllTicketTypes(int screenID)
        {
            return await _context.TicketTypes.Where(t => t.ScreenID == screenID).ToListAsync();
        }

        public async Task<TicketType> GetTicketTypeById(int id)
        {
            return await _context.TicketTypes.FindAsync(id);
        }

        public async Task CreateTicketType(TicketType ticketType)
        {
            _context.TicketTypes.Add(ticketType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketType(TicketType ticketType)
        {
            _context.Entry(ticketType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketType(int id)
        {
            var ticketType = await _context.TicketTypes.FindAsync(id);
            if (ticketType != null)
            {
                _context.TicketTypes.Remove(ticketType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateScreenSeats(int screenID, List<RowConfig> rowConfigs)
        {
            var seats = new List<Seat>();

            foreach (var rowConfig in rowConfigs)
            {
                char currentRow = rowConfig.RowLetter[0];

                for (int columnIndex = 1; columnIndex <= rowConfig.ColumnCount; columnIndex++)
                {
                    var seat = new Seat
                    {
                        ScreenID = screenID,
                        SeatNumber = $"{currentRow}{columnIndex}",
                        SeatRow = currentRow.ToString(),
                        SeatColumn = columnIndex,
                        TicketTypeID = rowConfig.TicketTypeID,
                        IsActive = true
                    };

                    seats.Add(seat);
                }
            }

            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Seat>> GetSeatsByScreen(int screenID)
        {
            try
            {
                return await _context.Seats
                    .Where(s => s.ScreenID == screenID && s.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("An error occurred while retrieving seats.", ex);
            }
        }



    }
}
