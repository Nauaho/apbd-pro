﻿using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{

    public interface IStocksRepository
    {
        public Task AddTickerDetailsAsync(TickerDetails tickerDetails);
        public Task AddTickerOHLCAsync(IEnumerable<TickerOHLC> tickerOhlc);
        public Task AddTickerOpenCloseAsync(TickerOpenClose tickerOpenClose);
        public Task<IEnumerable<TickerOHLC>?> GetAggregationAsync(string ticker, int multiplier, string timespan, DateOnly from, DateOnly to);
        public Task<TickerDetails?> GetTickersDetailsAsync(string ticker);
        public Task<TickerOpenClose?> GetTickersOpenCloseAsync(string ticker, DateOnly date);
    }
    public class StocksRepository : IStocksRepository
    {
        private readonly ProContext _context;
        public StocksRepository(ProContext context)
        {
            _context = context;
        }

        public async Task<TickerDetails?> GetTickersDetailsAsync(string ticker)
        {
            var a = _context.TickerDetails.Where(e => e.Ticker == ticker);
            if(!a.Any())
                return null;
            else
                return await a.FirstAsync();
        }

        public async Task<TickerOpenClose?> GetTickersOpenCloseAsync(string ticker, DateOnly date)
        {
            var d = date.ToDateTime(TimeOnly.MinValue);
            var a = _context.TickerOpenClose.Where(t => t.Symbol == ticker && t.From == d);
            if (!a.Any())
                return null;
            else
                return await a.FirstAsync();
        }

        public async Task<IEnumerable<TickerOHLC>?> GetAggregationAsync(string ticker, int multiplier, string timespan, DateOnly from, DateOnly to)
        {
            var uTo = new DateTimeOffset(to.ToDateTime(TimeOnly.MinValue)).ToUnixTimeMilliseconds();
            var uFrom = new DateTimeOffset(from.ToDateTime(TimeOnly.MinValue)).ToUnixTimeMilliseconds();

            var a = await _context.TickerOHLC.Where(t => t.Symbol == ticker && t.Multuplier == multiplier && t.Timespan == timespan && t.T <= uTo && t.T >= uFrom).ToListAsync();
            if(a.Count == 0) 
                return null;
            else
                return a;
        }

        public async Task AddTickerDetailsAsync(TickerDetails tickerDetails)
        {
            if( await _context.TickerDetails.ContainsAsync(tickerDetails) )
                _context.TickerDetails.Update(tickerDetails);
            else
                await _context.TickerDetails.AddAsync(tickerDetails);
            await _context.SaveChangesAsync();
        }

        public async Task AddTickerOpenCloseAsync(TickerOpenClose tickerOpenClose)
        {
            if( await _context.TickerOpenClose.ContainsAsync(tickerOpenClose) )
                _context.TickerOpenClose.Update(tickerOpenClose);
            else
                await _context.TickerOpenClose.AddAsync(tickerOpenClose);
            await _context.SaveChangesAsync();
        }
        public async Task AddTickerOHLCAsync(IEnumerable<TickerOHLC> tickerOhlc)
        {
            foreach(TickerOHLC tickerOHLC in tickerOhlc)
                if(await _context.TickerOHLC.ContainsAsync(tickerOHLC))
                    _context.TickerOHLC.Update(tickerOHLC);
                else
                   await _context.TickerOHLC.AddAsync(tickerOHLC);
            
            await _context.SaveChangesAsync();
        }
    }
}
