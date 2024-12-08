using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace NZWalksAPI.Controllers;

// https://localhost:portNumber/api/regions
[ApiController]
[Route("api/[controller]")]

public class RegionsController : ControllerBase
{
    private readonly NZWalksDBContext dbContext;
    private readonly IRegionRepository regionRepository;
    private readonly IMapper mapper;

    public RegionsController(NZWalksDBContext dbContext, IRegionRepository regionRepository, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.regionRepository = regionRepository;
        this.mapper = mapper;
    }
    // GET ALL REGIONS
    // https://localhost:portNumber/api/regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Get all regions from database
        var regionsDomain = await regionRepository.GetAllAsync();

        //Map domain models to DTOs
        var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

        return Ok(regionsDto);
    }

    // GET REGION BY ID
    // https://localhost:portNumber/api/regions/id
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        // var region = dbContext.Regions.Find(id);
        var regionDomain = await regionRepository.GetByIdAsync(id);
        //Get region  domain model from database
        if (regionDomain == null)
        {
            return NotFound();
        }

        //Map/ convert domain model to region DTO
        var regionDto = new RegionDto()
        {
            Id = regionDomain.Id,
            Name = regionDomain.Name,
            Code = regionDomain.Code,
            RegionImageUrl = regionDomain.RegionImageUrl
        };

        //Return DTO to client
        return Ok(regionDto);
    }

    // POST: /api/regions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = new Region
        {
            Id = Guid.NewGuid().ToString(),
            Name = addRegionRequestDto.Name,
            Code = addRegionRequestDto.Code,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        //Use domain model to create region
        regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

        //Map domain model back to DTO
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }


    // PUT update region
    [HttpPut]
    [Route("{id}")]  // Add guid constraint to route
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRegionRequestDto updateRegionRequest)
    {
        //MAP dto to domain model
        var regionDomainModel = new Region
        {
            Code = updateRegionRequest.Code,
            Name = updateRegionRequest.Name,
            RegionImageUrl = updateRegionRequest.RegionImageUrl
        };
        regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

        if (regionDomainModel == null)
        {
            return NotFound();
        }
        // Convert domain model back to DTO
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }

    //DELETE: /api/regions/{id}
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        //Check if region exists
        var regionDomainModel = await regionRepository.DeleteAsync(id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        //Optional " return deleted region
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }
}
