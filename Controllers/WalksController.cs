
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
// api/walks
[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IWalkRepository walkRepository;

    public WalksController(IMapper mapper, IWalkRepository walkRepository)
    {
        this.mapper = mapper;
        this.walkRepository = walkRepository;
    }
    // CREATE walk
    // api/walks
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        // MAP Dto to Domain model
        var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
        await walkRepository.CreateAsync(walkDomainModel);

        //Map domain model into dto
        return Ok(mapper.Map<WalkDto>(walkDomainModel));
    }

    // GET WALKS
    //api/walks?filterOn=Name 
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
    {
        var walksDomainModel = await walkRepository.GetAllAsync();

        //Map domain model into dto
        return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    // GET WALK BY ID
    //api/walks/{id}
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var walkDomainModel = await walkRepository.GetByIdAsync(id);

        if (walkDomainModel == null)
        {
            return NotFound();
        }

        //Map domain model into dto
        return Ok(mapper.Map<WalkDto>(walkDomainModel));
    }

    // Update Walk by Id
    //api/walks/{id}
    [HttpPut]
    [Route("{id}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] string id, UpdateWalkRequestDto updateWalkRequestDto)
    {
        //Map dto to domain model
        var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
        walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
        if (walkDomainModel == null)
        {
            return NotFound();
        }

        //Map domain model into dto
        return Ok(mapper.Map<WalkDto>(walkDomainModel));
    }

    // DELETE WALK BY ID
    //api/walks/{id}
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
        if (deletedWalkDomainModel == null)
        {
            return NotFound();
        }
        //Map domain model into dto
        return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
    }
}