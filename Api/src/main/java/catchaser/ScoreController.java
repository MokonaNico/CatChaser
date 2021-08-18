package catchaser;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

@RestController
public class ScoreController {

    @Autowired
    ScoreRepository scoreRepository;

    @PostMapping("/")
    public void addScore(@RequestBody Score score){
        scoreRepository.save(score);
    }

    @GetMapping("/")
    public List<Score> getScores(){
        List<Score> scores = scoreRepository.findAll();
        scores.sort(new Comparator<Score>() {
            @Override
            public int compare(Score o1, Score o2) {
                return o2.score - o1.score;
            }
        });
        if(scores.size() > 10)
            scores = scores.subList(0,10);
        return scores;
    }

}
