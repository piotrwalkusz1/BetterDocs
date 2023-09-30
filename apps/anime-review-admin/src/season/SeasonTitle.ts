import { Season as TSeason } from "../api/season/Season";

export const SEASON_TITLE_FIELD = "name";

export const SeasonTitle = (record: TSeason): string => {
  return record.name?.toString() || String(record.id);
};
