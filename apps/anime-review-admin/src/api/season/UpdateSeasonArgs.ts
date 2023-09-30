import { SeasonWhereUniqueInput } from "./SeasonWhereUniqueInput";
import { SeasonUpdateInput } from "./SeasonUpdateInput";

export type UpdateSeasonArgs = {
  where: SeasonWhereUniqueInput;
  data: SeasonUpdateInput;
};
