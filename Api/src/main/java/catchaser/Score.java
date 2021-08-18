package catchaser;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

@Entity
public class Score {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    public int id;

    public String name;

    public int score;

    public Score(){

    }

    public Score(String name, int score){
        this.name = name;
        this.score = score;
    }
}
