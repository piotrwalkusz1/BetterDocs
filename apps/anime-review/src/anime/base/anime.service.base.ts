/*
------------------------------------------------------------------------------ 
This code was generated by Amplication. 
 
Changes to this file will be lost if the code is regenerated. 

There are other ways to to customize your code, see this doc to learn more
https://docs.amplication.com/how-to/custom-code

------------------------------------------------------------------------------
  */
import { PrismaService } from "../../prisma/prisma.service";
import { Prisma, Anime, Season } from "@prisma/client";

export class AnimeServiceBase {
  constructor(protected readonly prisma: PrismaService) {}

  async count<T extends Prisma.AnimeCountArgs>(
    args: Prisma.SelectSubset<T, Prisma.AnimeCountArgs>
  ): Promise<number> {
    return this.prisma.anime.count(args);
  }

  async findMany<T extends Prisma.AnimeFindManyArgs>(
    args: Prisma.SelectSubset<T, Prisma.AnimeFindManyArgs>
  ): Promise<Anime[]> {
    return this.prisma.anime.findMany(args);
  }
  async findOne<T extends Prisma.AnimeFindUniqueArgs>(
    args: Prisma.SelectSubset<T, Prisma.AnimeFindUniqueArgs>
  ): Promise<Anime | null> {
    return this.prisma.anime.findUnique(args);
  }
  async create<T extends Prisma.AnimeCreateArgs>(
    args: Prisma.SelectSubset<T, Prisma.AnimeCreateArgs>
  ): Promise<Anime> {
    return this.prisma.anime.create<T>(args);
  }
  async update<T extends Prisma.AnimeUpdateArgs>(
    args: Prisma.SelectSubset<T, Prisma.AnimeUpdateArgs>
  ): Promise<Anime> {
    return this.prisma.anime.update<T>(args);
  }
  async delete<T extends Prisma.AnimeDeleteArgs>(
    args: Prisma.SelectSubset<T, Prisma.AnimeDeleteArgs>
  ): Promise<Anime> {
    return this.prisma.anime.delete(args);
  }

  async findSeasons(
    parentId: string,
    args: Prisma.SeasonFindManyArgs
  ): Promise<Season[]> {
    return this.prisma.anime
      .findUniqueOrThrow({
        where: { id: parentId },
      })
      .seasons(args);
  }
}
