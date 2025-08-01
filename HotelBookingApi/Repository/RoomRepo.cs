﻿using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private AppDbContext db;

       public RoomRepo (AppDbContext db)
        {
            this.db = db;
        }

        public List<Room> GetAll()
        {
            return db.Rooms.Include(se => se.Bookings).Include(se => se.Hotel).ToList();
        }
        public Room GetbyId(int id)
        {
            return db.Rooms.Include(se => se.Bookings).Include(se => se.Hotel).FirstOrDefault(se => se.Id == id);

        }

        public void Add(Room r)
        {
            db.Rooms.Add(r);

        }
        public void Update(Room r)
        {
            db.Entry(r).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        }
        public void Delete(Room r)
        {
            db.Rooms.Remove(r);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public IEnumerable<Room> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            var unavailableRoomIds = db.Bookings
                .Where(b => !(endDate <= b.CheckInDate || startDate >= b.CheckOutDate))
                .Select(b => b.RoomId)
                .ToList();

            return db.Rooms.Where(r => !unavailableRoomIds.Contains(r.Id));
        }

    }
}
