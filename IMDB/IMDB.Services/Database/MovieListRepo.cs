﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMDB.DataLayer;
using IMDB.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using static IMDB.Domain.DTOs.MovieDTO;

namespace IMDB.Services.Database
{
    public class MovieListRepo
    {
        private ContextDB _context { get; }
        public MovieListRepo(ContextDB context)
        {
            _context = context;
        }

        public async Task<int> AddToWatchList(WatchList movie)
        {
            await _context.WatchLists.AddAsync(movie);
            return movie.Id;
        }

        public void RemoveFromWatchList(WatchList movie)
        {
            _context.WatchLists.Remove(movie);
        }
        public async Task RemoveFromWatchList(int userId , int movieId)
        {
            _context.WatchLists.Remove(await _context.WatchLists.FirstOrDefaultAsync(m=>m.MovieId == movieId && m.UserId == userId));
        }
        public async Task<int> AddFavouriteList(string Title,int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            var listMovie = new FavouriteList()
            {
                CreateDate = DateTime.Now,
                Title = Title,
                UserId = UserId,
                UserName = user.Name,
                User = user,
            };
            await _context.FavouriteLists.AddAsync(listMovie);
            return listMovie.Id;
        }
        public async Task<int> AddFavouriteList(FavouriteList favouriteList)
        {
            await _context.FavouriteLists.AddAsync(favouriteList);
            return favouriteList.Id;
        }
        public async Task<int> AddMovieToList(int listId,int movieId)
        {
            FavouriteMovie movie = new FavouriteMovie() { FavouriteListId = listId,MovieId = movieId };
            await _context.FavouriteMovies.AddAsync(movie);
            return movie.Id;
        }
        public async Task RemoveMovieFromList(int movieId)
        {
            _context.FavouriteMovies.Remove(await _context.FavouriteMovies.FindAsync(movieId));
        }
        public async Task RemoveMovieFromList(FavouriteMovie movie)
        {
            _context.FavouriteMovies.Remove(movie);
        }
        public async Task<List<FavouriteList>> GetMovieListsById(int userId)
        {
            return await _context.FavouriteLists.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<List<WatchList>> GetWatchListById(int userId)
        {
            return await _context.WatchLists.Where(n => n.UserId == userId).ToListAsync();

        }

    }
}