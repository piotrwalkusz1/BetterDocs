import { Anime as TAnime } from "../api/anime/Anime";

export const ANIME_TITLE_FIELD = "name";

export const AnimeTitle = (record: TAnime): string => {
  return record.name?.toString() || String(record.id);
};
