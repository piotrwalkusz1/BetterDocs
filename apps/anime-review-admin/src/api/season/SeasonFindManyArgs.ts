import { SeasonWhereInput } from "./SeasonWhereInput";
import { SeasonOrderByInput } from "./SeasonOrderByInput";

export type SeasonFindManyArgs = {
  where?: SeasonWhereInput;
  orderBy?: Array<SeasonOrderByInput>;
  skip?: number;
  take?: number;
};
