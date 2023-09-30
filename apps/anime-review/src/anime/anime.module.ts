import { Module, forwardRef } from "@nestjs/common";
import { AuthModule } from "../auth/auth.module";
import { AnimeModuleBase } from "./base/anime.module.base";
import { AnimeService } from "./anime.service";
import { AnimeController } from "./anime.controller";
import { AnimeResolver } from "./anime.resolver";

@Module({
  imports: [AnimeModuleBase, forwardRef(() => AuthModule)],
  controllers: [AnimeController],
  providers: [AnimeService, AnimeResolver],
  exports: [AnimeService],
})
export class AnimeModule {}
